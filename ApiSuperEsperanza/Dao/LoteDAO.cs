using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class LoteDAO
    {
        private readonly ConexionDB _conexion;

        public LoteDAO(ConexionDB conexion)
        {
            _conexion = conexion;
        }

        public async Task<Lote?> ObtenerLotePorIdAsync(int idLote)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_OBTENER_LOTE_POR_ID, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Lote", idLote);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Lote
                {
                    Id_Lote = Convert.ToInt32(reader["Id_Lote"]),
                    CodigoLote = reader["CodigoLote"].ToString() ?? "",
                    Id_Producto = Convert.ToInt32(reader["Id_Producto"]),
                    Id_Compra = reader["Id_Compra"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Compra"]),
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                    FechaVencimiento = reader["FechaVencimiento"] is DBNull ? null : Convert.ToDateTime(reader["FechaVencimiento"]),
                    Estado = Convert.ToBoolean(reader["Estado"]),
                    FechaCreacion = reader["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaCreacion"]),
                    Id_UsuarioCreacion = reader["Id_UsuarioCreacion"] is DBNull ? 0 : Convert.ToInt32(reader["Id_UsuarioCreacion"])
                };
            }
            return null;
        }

        public async Task<int> InsertarLoteAsync(Lote lote)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_LOTE, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CodigoLote", lote.CodigoLote);
            cmd.Parameters.AddWithValue("@Id_Producto", lote.Id_Producto);
            cmd.Parameters.AddWithValue("@Id_Compra", lote.Id_Compra);
            cmd.Parameters.AddWithValue("@Cantidad", lote.Cantidad);
            // @FechaIngreso no se pasa porque el SP usa GETDATE() automáticamente
            cmd.Parameters.AddWithValue("@FechaVencimiento", (object?)lote.FechaVencimiento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", lote.Id_UsuarioCreacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Inserta un lote dentro de una transacción existente (para uso en compras)
        /// </summary>
        public async Task<int> InsertarLoteAsync(Lote lote, SqlConnection cn, SqlTransaction transaction)
        {
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_LOTE, cn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CodigoLote", lote.CodigoLote);
            cmd.Parameters.AddWithValue("@Id_Producto", lote.Id_Producto);
            cmd.Parameters.AddWithValue("@Id_Compra", lote.Id_Compra);
            cmd.Parameters.AddWithValue("@Cantidad", lote.Cantidad);
            // @FechaIngreso no se pasa porque el SP usa GETDATE() automáticamente
            cmd.Parameters.AddWithValue("@FechaVencimiento", (object?)lote.FechaVencimiento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", lote.Id_UsuarioCreacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<Lote>> ListarLotesPorProductoAsync(int idProducto)
        {
            var lista = new List<Lote>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_LOTES_POR_PRODUCTO, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Producto", idProducto);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Lote
                {
                    Id_Lote = Convert.ToInt32(reader["Id_Lote"]),
                    CodigoLote = reader["CodigoLote"].ToString() ?? "",
                    Id_Producto = idProducto,
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                    FechaVencimiento = reader["FechaVencimiento"] is DBNull ? null : Convert.ToDateTime(reader["FechaVencimiento"]),
                    Estado = true
                });
            }
            return lista;
        }

        /// <summary>
        /// Marca los lotes vencidos como inactivos usando el procedimiento almacenado
        /// </summary>
        public async Task<int> MarcarLotesVencidosAsync()
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_MARCAR_LOTES_VENCIDOS, cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // El procedimiento no devuelve un valor, solo ejecuta
            await cmd.ExecuteNonQueryAsync();
            
            // Devolvemos 0 ya que el procedimiento no retorna cantidad de lotes marcados
            // Si necesitas saber cuántos se marcaron, deberías modificar el SP para que retorne ese valor
            return 0;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}
