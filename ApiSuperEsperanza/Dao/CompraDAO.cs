using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class CompraDAO
    {
        private readonly ConexionDB _conexion;
        private readonly LoteDAO _loteDAO;

        public CompraDAO(ConexionDB conexion, LoteDAO loteDAO)
        {
            _conexion = conexion;
            _loteDAO = loteDAO;
        }

        /// <summary>
        /// Genera un código único de 10 caracteres con el patrón: Prefijo (3 caracteres) + 7 caracteres aleatorios
        /// </summary>
        private static string GenerarCodigoUnico(string prefijo)
        {
            // Usar Guid para obtener una semilla única basada en tiempo y hardware
            var random = new Random(Guid.NewGuid().GetHashCode() ^ Environment.TickCount);
            
            // Caracteres permitidos: números (0-9) y letras mayúsculas (A-Z)
            const string caracteres = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            // Generar código de 10 caracteres: Prefijo (3) + 7 aleatorios
            var codigo = new System.Text.StringBuilder(10);
            codigo.Append(prefijo); // Prefijo (3 caracteres)
            
            // Generar 7 caracteres aleatorios alfanuméricos
            for (int i = 0; i < 7; i++)
            {
                codigo.Append(caracteres[random.Next(caracteres.Length)]);
            }
            
            return codigo.ToString(); // Total: 10 caracteres
        }

        public async Task<int> InsertarCompraAsync(Compra compra)
        {
            using var cn = await GetOpenConnectionAsync();
            // Usar ReadCommitted para operaciones de compra
            using var transaction = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                // Insertar la compra (maestro)
                using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_COMPRA, cn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CodigoCompra", compra.CodigoCompra);
                cmd.Parameters.AddWithValue("@Id_Proveedor", compra.Id_Proveedor);
                
                // Asegurar que la fecha esté en el rango válido de SQL Server y usar solo la fecha
                // Normalizar la fecha para evitar problemas de zona horaria
                var fechaRecibida = compra.FechaCompra;
                
                // Log para debugging
                System.Diagnostics.Debug.WriteLine($"Fecha recibida en DAO: {fechaRecibida:yyyy-MM-dd HH:mm:ss}, Kind: {fechaRecibida.Kind}");
                
                // Extraer solo la fecha, manejando diferentes DateTimeKind
                DateTime fechaSoloFecha;
                if (fechaRecibida.Kind == DateTimeKind.Utc || fechaRecibida.Kind == DateTimeKind.Local)
                {
                    // Si viene con zona horaria, crear una nueva fecha sin zona horaria
                    fechaSoloFecha = new DateTime(fechaRecibida.Year, fechaRecibida.Month, fechaRecibida.Day, 0, 0, 0, DateTimeKind.Unspecified);
                }
                else
                {
                    fechaSoloFecha = fechaRecibida.Date;
                }
                
                var minDate = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
                var maxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified);
                
                // Validar rango
                if (fechaSoloFecha < minDate.Date || fechaSoloFecha > maxDate.Date)
                {
                    throw new ArgumentOutOfRangeException(nameof(compra.FechaCompra), 
                        $"La fecha debe estar entre {minDate:yyyy-MM-dd} y {maxDate:yyyy-MM-dd}. Fecha recibida: {fechaRecibida:yyyy-MM-dd HH:mm:ss} (Kind: {fechaRecibida.Kind})");
                }
                
                // Crear una fecha completamente nueva sin zona horaria para SQL Server
                // Asegurar que sea exactamente medianoche sin zona horaria
                var fechaNormalizada = new DateTime(
                    fechaSoloFecha.Year,
                    fechaSoloFecha.Month,
                    fechaSoloFecha.Day,
                    0, 0, 0,
                    DateTimeKind.Unspecified
                );
                
                // Validación final antes de enviar a SQL Server
                if (fechaNormalizada < minDate || fechaNormalizada > maxDate)
                {
                    throw new ArgumentOutOfRangeException(nameof(compra.FechaCompra), 
                        $"La fecha normalizada está fuera del rango válido. Fecha: {fechaNormalizada:yyyy-MM-dd HH:mm:ss}");
                }
                
                System.Diagnostics.Debug.WriteLine($"Fecha normalizada para SQL: {fechaNormalizada:yyyy-MM-dd HH:mm:ss}, Kind: {fechaNormalizada.Kind}");
                
                // Usar SqlParameter explícitamente para asegurar el tipo correcto
                var paramFecha = new SqlParameter("@FechaCompra", System.Data.SqlDbType.DateTime)
                {
                    Value = fechaNormalizada
                };
                cmd.Parameters.Add(paramFecha);
                cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", compra.Id_UsuarioCreacion);

                var idCompra = await cmd.ExecuteScalarAsync();
                if (idCompra == null || idCompra == DBNull.Value)
                {
                    throw new InvalidOperationException("No se pudo obtener el ID de la compra insertada.");
                }
                var idCompraInt = Convert.ToInt32(idCompra);
                if (idCompraInt <= 0)
                {
                    throw new InvalidOperationException($"ID de compra inválido: {idCompraInt}");
                }
                compra.Id_Compra = idCompraInt;

                // Insertar los detalles
                if (compra.Detalles != null && compra.Detalles.Any())
                {
                    foreach (var detalle in compra.Detalles)
                    {
                        detalle.Id_Compra = idCompraInt;
                        await InsertarDetalleCompraAsync(detalle, cn, transaction);
                    }
                }

                // Crear lotes dentro de la misma transacción para cada detalle
                if (compra.Detalles != null && compra.Detalles.Any())
                {
                    // Usar la fecha normalizada que ya validamos arriba
                    var fechaIngreso = fechaNormalizada.Date;

                    foreach (var detalle in compra.Detalles)
                    {
                        // Validar que la cantidad sea positiva
                        if (detalle.Cantidad <= 0)
                        {
                            throw new ArgumentException($"La cantidad debe ser mayor a cero para el producto ID {detalle.Id_Producto}.");
                        }

                        // Validar que el precio sea positivo
                        if (detalle.PrecioUnitario <= 0)
                        {
                            throw new ArgumentException($"El precio unitario debe ser mayor a cero para el producto ID {detalle.Id_Producto}.");
                        }

                        var lote = new Lote
                        {
                            CodigoLote = GenerarCodigoUnico("LOT"), // LOT + 7 aleatorios = 10 caracteres
                            Id_Producto = detalle.Id_Producto,
                            Id_Compra = idCompraInt,
                            Cantidad = detalle.Cantidad,
                            FechaIngreso = fechaIngreso,
                            FechaVencimiento = null,
                            Id_UsuarioCreacion = compra.Id_UsuarioCreacion
                        };

                        await _loteDAO.InsertarLoteAsync(lote, cn, transaction);
                    }
                }

                transaction.Commit();
                return idCompraInt;
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

        private async Task InsertarDetalleCompraAsync(DetalleCompra detalle, SqlConnection cn, SqlTransaction transaction)
        {
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_DETALLE_COMPRA, cn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Compra", detalle.Id_Compra);
            cmd.Parameters.AddWithValue("@Id_Producto", detalle.Id_Producto);
            cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", detalle.Id_UsuarioCreacion);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<DetalleCompra>> ListarDetallesPorCompraAsync(int idCompra)
        {
            var lista = new List<DetalleCompra>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_DETALLES_POR_COMPRA, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Compra", idCompra);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new DetalleCompra
                {
                    Id_DetalleCompra = Convert.ToInt32(reader["Id_DetalleCompra"]),
                    Id_Compra = idCompra,
                    Id_Producto = Convert.ToInt32(reader["Id_Producto"]),
                    NombreProducto = reader["NombreProducto"].ToString(),
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                    Subtotal = Convert.ToDecimal(reader["Subtotal"])
                });
            }
            return lista;
        }

        public async Task<bool> ActualizarDetalleCompraAsync(DetalleCompra detalle, int idUsuarioModificacion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ACTUALIZAR_DETALLE_COMPRA, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_DetalleCompra", detalle.Id_DetalleCompra);
            cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", idUsuarioModificacion);

            var result = await cmd.ExecuteScalarAsync();
            return result != null && Convert.ToInt32(result) == 1;
        }

        public async Task<bool> EliminarDetalleCompraAsync(int idDetalleCompra)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ELIMINAR_DETALLE_COMPRA, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_DetalleCompra", idDetalleCompra);

            var result = await cmd.ExecuteScalarAsync();
            return result != null && Convert.ToInt32(result) == 1;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}

