using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class FacturaDAO
    {
        private readonly ConexionDB _conexion;

        public FacturaDAO(ConexionDB conexion)
        {
            _conexion = conexion;
        }

        public async Task<int> InsertarFacturaAsync(Factura factura)
        {
            using var cn = await GetOpenConnectionAsync();
            // Usar Serializable para operaciones críticas de stock que involucran múltiples lotes
            // Esto previene lecturas fantasma y asegura que el stock no cambie durante la transacción
            using var transaction = cn.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                // Insertar la factura (maestro)
                using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_FACTURA, cn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CodigoFactura", factura.CodigoFactura);
                cmd.Parameters.AddWithValue("@Id_Cliente", (object?)factura.Id_Cliente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id_Sesion", factura.Id_Sesion);
                cmd.Parameters.AddWithValue("@NumeroFactura", factura.NumeroFactura);
                cmd.Parameters.AddWithValue("@Subtotal", factura.Subtotal);
                cmd.Parameters.AddWithValue("@Descuento", factura.Descuento);
                cmd.Parameters.AddWithValue("@Impuesto", factura.Impuesto);
                cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", factura.Id_UsuarioCreacion);

                var idFactura = await cmd.ExecuteScalarAsync();
                if (idFactura == null || idFactura == DBNull.Value)
                {
                    throw new InvalidOperationException("No se pudo obtener el ID de la factura insertada.");
                }
                var idFacturaInt = Convert.ToInt32(idFactura);
                if (idFacturaInt <= 0)
                {
                    throw new InvalidOperationException($"ID de factura inválido: {idFacturaInt}");
                }
                factura.Id_Factura = idFacturaInt;

                // Insertar los detalles
                if (factura.Detalles != null && factura.Detalles.Any())
                {
                    // Agrupar detalles por lote para evitar problemas de concurrencia
                    // Si hay múltiples detalles del mismo lote, sumar las cantidades
                    var detallesAgrupados = factura.Detalles
                        .GroupBy(d => new { d.Id_Lote, d.PrecioUnitario })
                        .Select(g => new DetalleFactura
                        {
                            Id_Factura = idFacturaInt,
                            Id_Lote = g.Key.Id_Lote,
                            Cantidad = g.Sum(d => d.Cantidad),
                            PrecioUnitario = g.Key.PrecioUnitario,
                            Subtotal = g.Sum(d => d.Subtotal),
                            Id_UsuarioCreacion = g.First().Id_UsuarioCreacion
                        })
                        .ToList();

                    // Validar que no haya cantidades negativas o cero después del agrupamiento
                    foreach (var detalle in detallesAgrupados)
                    {
                        if (detalle.Cantidad <= 0)
                        {
                            throw new InvalidOperationException($"La cantidad debe ser mayor a cero para el lote ID {detalle.Id_Lote}");
                        }
                        
                        if (detalle.PrecioUnitario <= 0)
                        {
                            throw new InvalidOperationException($"El precio unitario debe ser mayor a cero para el lote ID {detalle.Id_Lote}");
                        }
                    }

                    // El stored procedure maneja toda la validación y bloqueo
                    // No hacer validación previa aquí porque libera el lock al cerrar el reader
                    // El SP tiene su propio UPDLOCK + HOLDLOCK que mantiene el lock durante toda la operación
                    // Procesar los detalles ordenados por Id_Lote para evitar deadlocks
                    System.Diagnostics.Debug.WriteLine($"=== Iniciando procesamiento de {detallesAgrupados.Count} detalles ===");
                    System.Diagnostics.Debug.WriteLine($"Factura ID: {idFacturaInt}");
                    System.Diagnostics.Debug.WriteLine($"Orden de procesamiento (por Id_Lote):");
                    foreach (var det in detallesAgrupados.OrderBy(d => d.Id_Lote))
                    {
                        System.Diagnostics.Debug.WriteLine($"  - Lote ID: {det.Id_Lote}, Cantidad: {det.Cantidad}, Precio: {det.PrecioUnitario}, Usuario: {det.Id_UsuarioCreacion}");
                    }
                    
                    var detallesOrdenados = detallesAgrupados.OrderBy(d => d.Id_Lote).ToList();
                    for (int i = 0; i < detallesOrdenados.Count; i++)
                    {
                        var detalle = detallesOrdenados[i];
                        System.Diagnostics.Debug.WriteLine($"");
                        System.Diagnostics.Debug.WriteLine($"=== Procesando detalle {i + 1} de {detallesOrdenados.Count} ===");
                        System.Diagnostics.Debug.WriteLine($"Lote ID: {detalle.Id_Lote}, Cantidad: {detalle.Cantidad}, Precio: {detalle.PrecioUnitario}");
                        
                        await InsertarDetalleFacturaAsync(detalle, cn, transaction);
                        
                        System.Diagnostics.Debug.WriteLine($"✓ Detalle {i + 1} procesado exitosamente: Lote ID {detalle.Id_Lote}");
                    }
                    System.Diagnostics.Debug.WriteLine($"");
                    System.Diagnostics.Debug.WriteLine($"=== Todos los detalles procesados exitosamente ===");
                }

                transaction.Commit();
                return idFacturaInt;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                // Manejar errores específicos de SQL Server
                if (ex.Number == 1205) // Deadlock
                {
                    throw new InvalidOperationException("Error de concurrencia: La operación fue interrumpida por otra transacción. Por favor, intente nuevamente.", ex);
                }
                else if (ex.Number == -2) // Timeout
                {
                    throw new InvalidOperationException("La operación tardó demasiado tiempo. Por favor, intente nuevamente.", ex);
                }
                else if (ex.Number == 50000) // RAISERROR personalizado
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }
                throw;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private async Task InsertarDetalleFacturaAsync(DetalleFactura detalle, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                // El stored procedure ahora maneja toda la validación y actualización del stock
                // con locks apropiados para evitar problemas de concurrencia
                // Log detallado para diagnóstico
                System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] Iniciando inserción:");
                System.Diagnostics.Debug.WriteLine($"  - Factura ID: {detalle.Id_Factura}");
                System.Diagnostics.Debug.WriteLine($"  - Lote ID: {detalle.Id_Lote}");
                System.Diagnostics.Debug.WriteLine($"  - Cantidad: {detalle.Cantidad}");
                System.Diagnostics.Debug.WriteLine($"  - Precio Unitario: {detalle.PrecioUnitario}");
                System.Diagnostics.Debug.WriteLine($"  - Usuario Creación: {detalle.Id_UsuarioCreacion}");
                
                using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_DETALLE_FACTURA, cn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                // Agregar parámetros de forma explícita para asegurar que se pasen correctamente
                var paramIdFactura = new SqlParameter("@Id_Factura", SqlDbType.Int) { Value = detalle.Id_Factura };
                var paramIdLote = new SqlParameter("@Id_Lote", SqlDbType.Int) { Value = detalle.Id_Lote };
                var paramCantidad = new SqlParameter("@Cantidad", SqlDbType.Int) { Value = detalle.Cantidad };
                var paramPrecioUnitario = new SqlParameter("@PrecioUnitario", SqlDbType.Decimal) { Value = detalle.PrecioUnitario, Precision = 18, Scale = 2 };
                var paramIdUsuario = new SqlParameter("@Id_UsuarioCreacion", SqlDbType.Int) { Value = detalle.Id_UsuarioCreacion };
                
                cmd.Parameters.Add(paramIdFactura);
                cmd.Parameters.Add(paramIdLote);
                cmd.Parameters.Add(paramCantidad);
                cmd.Parameters.Add(paramPrecioUnitario);
                cmd.Parameters.Add(paramIdUsuario);
                
                // Log de los parámetros que se están enviando
                System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] Parámetros enviados al SP:");
                System.Diagnostics.Debug.WriteLine($"  - @Id_Factura: {paramIdFactura.Value}");
                System.Diagnostics.Debug.WriteLine($"  - @Id_Lote: {paramIdLote.Value}");
                System.Diagnostics.Debug.WriteLine($"  - @Cantidad: {paramCantidad.Value}");
                System.Diagnostics.Debug.WriteLine($"  - @PrecioUnitario: {paramPrecioUnitario.Value}");
                System.Diagnostics.Debug.WriteLine($"  - @Id_UsuarioCreacion: {paramIdUsuario.Value}");

                // Ejecutar el SP - puede lanzar SqlException con RAISERROR
                System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] Ejecutando stored procedure...");
                try
                {
                    var result = await cmd.ExecuteScalarAsync();
                    var resultadoInt = result != null ? Convert.ToInt32(result) : -999;
                    System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] Resultado del SP: {resultadoInt}");
                    
                    // El SP devuelve 1 si es exitoso, -1 si hay error
                    if (result == null || resultadoInt < 0)
                    {
                        // Si llegamos aquí sin excepción, el SP devolvió -1 pero no lanzó RAISERROR
                        // Esto no debería pasar porque el SP siempre lanza RAISERROR antes de retornar -1
                        System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] ERROR: SP devolvió código negativo sin lanzar excepción");
                        throw new InvalidOperationException($"Error al insertar detalle de factura para lote ID {detalle.Id_Lote}, cantidad {detalle.Cantidad}. El stored procedure devolvió código: {resultadoInt}. Esto puede indicar un problema con el stored procedure.");
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] Detalle insertado exitosamente para Lote ID {detalle.Id_Lote}");
                }
                catch (SqlException sqlEx)
                {
                    // Capturar y loguear TODOS los errores de SQL antes de re-lanzarlos
                    System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] ERROR SQL capturado:");
                    System.Diagnostics.Debug.WriteLine($"  - Número de error: {sqlEx.Number}");
                    System.Diagnostics.Debug.WriteLine($"  - Mensaje: {sqlEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"  - Estado: {sqlEx.State}");
                    System.Diagnostics.Debug.WriteLine($"  - Severidad: {sqlEx.Class}");
                    if (sqlEx.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - Error interno: {sqlEx.InnerException.Message}");
                    }
                    
                    // Re-lanzar la excepción para que sea manejada por los bloques catch específicos
                    throw;
                }
            }
            catch (SqlException ex) when (ex.Number == 50000) // RAISERROR genera error 50000
            {
                // El stored procedure lanzó un RAISERROR con información detallada
                // El mensaje ya contiene toda la información necesaria (stock disponible, solicitado, etc.)
                System.Diagnostics.Debug.WriteLine($"[InsertarDetalleFacturaAsync] RAISERROR capturado (50000): {ex.Message}");
                // Propagar el mensaje original del SP - este es el mensaje más importante
                var mensajeSP = ex.Message;
                throw new InvalidOperationException(mensajeSP, ex);
            }
            catch (SqlException ex) when (ex.Number == 1205) // Deadlock
            {
                throw new InvalidOperationException($"Error de concurrencia al insertar detalle para lote ID {detalle.Id_Lote}. Por favor, intente nuevamente.", ex);
            }
            catch (SqlException ex)
            {
                // Otros errores de SQL (CHECK constraints, UNIQUE constraints, etc.)
                // Incluir el número de error y el mensaje completo para diagnóstico
                var mensajeError = $"Error SQL {ex.Number} al insertar detalle de factura para lote ID {detalle.Id_Lote}: {ex.Message}";
                if (ex.Number == 2627) // Violación de UNIQUE constraint
                {
                    mensajeError = $"Ya existe un detalle de factura para el lote ID {detalle.Id_Lote} en esta factura. Esto no debería ocurrir si los detalles están agrupados correctamente.";
                }
                throw new InvalidOperationException(mensajeError, ex);
            }
        }

        public async Task<IEnumerable<DetalleFactura>> ListarDetallesPorFacturaAsync(int idFactura)
        {
            var lista = new List<DetalleFactura>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_DETALLES_POR_FACTURA, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Factura", idFactura);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new DetalleFactura
                {
                    Id_DetalleFactura = Convert.ToInt32(reader["Id_Detalle"]),
                    Id_Factura = idFactura,
                    Id_Lote = Convert.ToInt32(reader["Id_Lote"]),
                    CodigoLote = reader["CodigoLote"].ToString(),
                    Id_Producto = 0, // No viene en el resultado, pero podemos obtenerlo del lote si es necesario
                    NombreProducto = reader["NombreProducto"].ToString(),
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                    Subtotal = Convert.ToDecimal(reader["Subtotal"])
                });
            }
            return lista;
        }

        public async Task<bool> AnularFacturaAsync(int idFactura, string motivo, int idUsuarioModificacion)
        {
            try
            {
                using var cn = await GetOpenConnectionAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_ANULAR_FACTURA, cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id_Factura", idFactura);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", idUsuarioModificacion);

                var result = await cmd.ExecuteScalarAsync();
                // El SP devuelve 1 si es exitoso, -1 si hay error (factura no existe o ya está anulada)
                if (result == null || Convert.ToInt32(result) < 0)
                {
                    return false;
                }
                return Convert.ToInt32(result) == 1;
            }
            catch (SqlException ex) when (ex.Number == 50000) // RAISERROR genera error 50000
            {
                // La factura no existe o ya está anulada
                return false;
            }
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}

