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
            cmd.Parameters.AddWithValue("@FechaVencimiento", (object?)lote.FechaVencimiento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", lote.Id_UsuarioCreacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Lote?> ObtenerLotePorIdAsync(int idLote)
        {
            using var cn = await GetOpenConnectionAsync();
            // Nota: Si existe el SP sp_ObtenerLotePorId, usarlo. Si no, usar SELECT directo
            // Por ahora usamos SELECT directo ya que el SP no fue proporcionado en los scripts
            using var cmd = new SqlCommand("SELECT * FROM Lote WHERE Id_Lote = @Id_Lote", cn);
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
                    Id_Producto = idProducto, // Ya sabemos que es este producto (viene del parámetro)
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                    FechaVencimiento = reader["FechaVencimiento"] is DBNull ? null : Convert.ToDateTime(reader["FechaVencimiento"]),
                    Estado = true // El SP sp_ListarLotesPorProducto solo devuelve lotes con Estado = 1
                    // Nota: El SP no devuelve Id_Compra, FechaCreacion, Id_UsuarioCreacion
                });
            }
            return lista;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}

