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
            // Usar ReadCommitted para evitar bloqueos excesivos y permitir manejo correcto de errores
            // El stored procedure maneja el bloqueo a nivel de fila con UPDLOCK + HOLDLOCK
            using var transaction = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
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
                    // Agrupar detalles por lote (el constraint UQ_DetalleFactura solo permite un detalle por lote por factura)
                    var detallesAgrupados = factura.Detalles
                        .GroupBy(d => d.Id_Lote)
                        .Select(g => new DetalleFactura
                        {
                            Id_Factura = idFacturaInt,
                            Id_Lote = g.Key,
                            Cantidad = g.Sum(d => d.Cantidad),
                            PrecioUnitario = g.Average(d => d.PrecioUnitario), // Precio promedio si hay diferentes precios
                            Subtotal = g.Sum(d => d.Subtotal),
                            Id_UsuarioCreacion = g.First().Id_UsuarioCreacion
                        })
                        .OrderBy(d => d.Id_Lote) // Ordenar para evitar deadlocks
                        .ToList();

                    // Procesar cada detalle
                    foreach (var detalle in detallesAgrupados)
                    {
                        await InsertarDetalleFacturaAsync(detalle, cn, transaction);
                    }
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
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_DETALLE_FACTURA, cn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            cmd.Parameters.AddWithValue("@Id_Factura", detalle.Id_Factura);
            cmd.Parameters.AddWithValue("@Id_Lote", detalle.Id_Lote);
            cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", detalle.Id_UsuarioCreacion);

            try
            {
                var result = await cmd.ExecuteScalarAsync();
                
                // El SP retorna 1 si es exitoso, -1 si hay error
                // Si hay error, el SP lanza RAISERROR que se captura como SqlException
                if (result != null && result != DBNull.Value)
                {
                    var resultadoInt = Convert.ToInt32(result);
                    if (resultadoInt < 0)
                    {
                        throw new InvalidOperationException($"Error al insertar detalle de factura para lote ID {detalle.Id_Lote}.");
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                // RAISERROR del stored procedure - el mensaje ya es descriptivo
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                // Otros errores de SQL
                throw new InvalidOperationException($"Error al insertar detalle de factura: {ex.Message}", ex);
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

