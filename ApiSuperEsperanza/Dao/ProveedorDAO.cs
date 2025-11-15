using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Dao.Interfaces;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class ProveedorDAO : RepositoryBase<Proveedor>, IRepository<Proveedor>
    {
        public ProveedorDAO(ConexionDB conexion) : base(conexion) { }

        public override async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            var lista = new List<Proveedor>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_PROVEEDORES, cn) { CommandType = CommandType.StoredProcedure };
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapReader(reader));
            }
            return lista;
        }

        public override async Task<Proveedor?> GetByIdAsync(int id)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_OBTENER_PROVEEDOR_POR_ID, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Proveedor", id);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapReader(reader) : null;
        }

        public override async Task<int> CreateAsync(Proveedor entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_PROVEEDOR, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CodigoProveedor", entity.CodigoProveedor);
            cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contacto", (object?)entity.Contacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", entity.Id_UsuarioCreacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null ? Convert.ToInt32(res) : 0;
        }

        public override async Task<bool> UpdateAsync(Proveedor entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ACTUALIZAR_PROVEEDOR, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Proveedor", entity.Id_Proveedor);
            cmd.Parameters.AddWithValue("@CodigoProveedor", entity.CodigoProveedor);
            cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contacto", (object?)entity.Contacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", entity.Estado);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", entity.Id_UsuarioModificacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        public override async Task<bool> DeleteAsync(int id, int idUsuarioModificacion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ELIMINAR_PROVEEDOR, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Proveedor", id);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", idUsuarioModificacion);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        private Proveedor MapReader(SqlDataReader reader)
        {
            var proveedor = new Proveedor
            {
                Id_Proveedor = Convert.ToInt32(reader["Id_Proveedor"]),
                CodigoProveedor = reader["CodigoProveedor"].ToString() ?? "",
                Nombre = reader["Nombre"].ToString() ?? "",
                Telefono = reader["Telefono"] is DBNull ? null : reader["Telefono"].ToString(),
                Email = reader["Email"] is DBNull ? null : reader["Email"].ToString(),
                Contacto = reader["Contacto"] is DBNull ? null : reader["Contacto"].ToString(),
                Estado = Convert.ToBoolean(reader["Estado"])
            };

            if (HasColumn(reader, "FechaCreacion"))
            {
                proveedor.FechaCreacion = reader["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaCreacion"]);
            }

            if (HasColumn(reader, "FechaModificacion"))
            {
                proveedor.FechaModificacion = reader["FechaModificacion"] is DBNull ? null : (DateTime?)Convert.ToDateTime(reader["FechaModificacion"]);
            }

            if (HasColumn(reader, "Id_UsuarioCreacion"))
            {
                proveedor.Id_UsuarioCreacion = reader["Id_UsuarioCreacion"] is DBNull ? 0 : Convert.ToInt32(reader["Id_UsuarioCreacion"]);
            }

            if (HasColumn(reader, "Id_UsuarioModificacion"))
            {
                proveedor.Id_UsuarioModificacion = reader["Id_UsuarioModificacion"] is DBNull ? null : Convert.ToInt32(reader["Id_UsuarioModificacion"]);
            }

            return proveedor;
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

