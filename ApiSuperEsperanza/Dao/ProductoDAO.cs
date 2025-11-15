using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Dao.Interfaces;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class ProductoDAO : RepositoryBase<Producto>, IRepository<Producto>
    {
        public ProductoDAO(ConexionDB conexion) : base(conexion) { }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            var lista = new List<Producto>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_PRODUCTOS, cn) { CommandType = CommandType.StoredProcedure };
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapReader(reader));
            }
            return lista;
        }

        public override async Task<Producto?> GetByIdAsync(int id)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_OBTENER_PRODUCTO_POR_ID, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Producto", id);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapReader(reader) : null;
        }

        public override async Task<int> CreateAsync(Producto entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_PRODUCTO, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CodigoProducto", entity.CodigoProducto);
            cmd.Parameters.AddWithValue("@Id_Categoria", entity.Id_Categoria);
            cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrecioVenta", entity.PrecioVenta);
            cmd.Parameters.AddWithValue("@StockActual", entity.StockActual);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", entity.Id_UsuarioCreacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null ? Convert.ToInt32(res) : 0;
        }

        public override async Task<bool> UpdateAsync(Producto entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ACTUALIZAR_PRODUCTO, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Producto", entity.Id_Producto);
            cmd.Parameters.AddWithValue("@CodigoProducto", entity.CodigoProducto);
            cmd.Parameters.AddWithValue("@Id_Categoria", entity.Id_Categoria);
            cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)entity.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrecioVenta", entity.PrecioVenta);
            cmd.Parameters.AddWithValue("@StockActual", entity.StockActual);
            cmd.Parameters.AddWithValue("@Estado", entity.Estado);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", entity.Id_UsuarioModificacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        public override async Task<bool> DeleteAsync(int id, int idUsuarioModificacion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ELIMINAR_PRODUCTO, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Producto", id);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", idUsuarioModificacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        private Producto MapReader(SqlDataReader reader)
        {
            var producto = new Producto
            {
                Id_Producto = Convert.ToInt32(reader["Id_Producto"]),
                CodigoProducto = reader["CodigoProducto"].ToString() ?? "",
                Id_Categoria = Convert.ToInt32(reader["Id_Categoria"]),
                NombreCategoria = HasColumn(reader, "NombreCategoria") ? (reader["NombreCategoria"] is DBNull ? null : reader["NombreCategoria"].ToString()) : null,
                Nombre = reader["Nombre"].ToString() ?? "",
                Descripcion = reader["Descripcion"] is DBNull ? null : reader["Descripcion"].ToString(),
                PrecioVenta = Convert.ToDecimal(reader["PrecioVenta"]),
                StockActual = Convert.ToInt32(reader["StockActual"]),
                Estado = Convert.ToBoolean(reader["Estado"])
            };

            if (HasColumn(reader, "CostoPromedio"))
            {
                producto.CostoPromedio = reader["CostoPromedio"] is DBNull ? 0 : Convert.ToDecimal(reader["CostoPromedio"]);
            }

            if (HasColumn(reader, "FechaCreacion"))
            {
                producto.FechaCreacion = reader["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaCreacion"]);
            }

            if (HasColumn(reader, "FechaModificacion"))
            {
                producto.FechaModificacion = reader["FechaModificacion"] is DBNull ? null : (DateTime?)Convert.ToDateTime(reader["FechaModificacion"]);
            }

            if (HasColumn(reader, "Id_UsuarioCreacion"))
            {
                producto.Id_UsuarioCreacion = reader["Id_UsuarioCreacion"] is DBNull ? 0 : Convert.ToInt32(reader["Id_UsuarioCreacion"]);
            }

            if (HasColumn(reader, "Id_UsuarioModificacion"))
            {
                producto.Id_UsuarioModificacion = reader["Id_UsuarioModificacion"] is DBNull ? null : Convert.ToInt32(reader["Id_UsuarioModificacion"]);
            }

            return producto;
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}

