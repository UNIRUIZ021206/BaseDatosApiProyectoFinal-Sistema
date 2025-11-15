using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class CompraDAO
    {
        private readonly ConexionDB _conexion;

        public CompraDAO(ConexionDB conexion)
        {
            _conexion = conexion;
        }

        public async Task<int> InsertarCompraAsync(Compra compra)
        {
            using var cn = await GetOpenConnectionAsync();
            using var transaction = cn.BeginTransaction();
            try
            {
                // Insertar la compra (maestro)
                using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_COMPRA, cn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CodigoCompra", compra.CodigoCompra);
                cmd.Parameters.AddWithValue("@Id_Proveedor", compra.Id_Proveedor);
                cmd.Parameters.AddWithValue("@FechaCompra", compra.FechaCompra);
                cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", compra.Id_UsuarioCreacion);

                var idCompra = await cmd.ExecuteScalarAsync();
                var idCompraInt = Convert.ToInt32(idCompra);
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

                transaction.Commit();
                return idCompraInt;
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

