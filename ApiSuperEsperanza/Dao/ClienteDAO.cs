using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Dao.Interfaces;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class ClienteDAO : RepositoryBase<Cliente>, IRepository<Cliente>
    {
        public ClienteDAO(ConexionDB conexion) : base(conexion) { }

        public override async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var lista = new List<Cliente>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_CLIENTES, cn) { CommandType = CommandType.StoredProcedure };
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapReader(reader));
            }
            return lista;
        }

        public override async Task<Cliente?> GetByIdAsync(int id)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_OBTENER_CLIENTE_POR_ID, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Cliente", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public override async Task<int> CreateAsync(Cliente entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_CLIENTE, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CodigoCliente", entity.CodigoCliente);
            cmd.Parameters.AddWithValue("@P_Nombre", entity.P_Nombre);
            cmd.Parameters.AddWithValue("@S_Nombre", (object?)entity.S_Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@P_Apellido", entity.P_Apellido);
            cmd.Parameters.AddWithValue("@S_Apellido", (object?)entity.S_Apellido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Direccion", (object?)entity.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TipoMembresia", (object?)entity.TipoMembresia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id_UsuarioCreacion", entity.Id_UsuarioCreacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public override async Task<bool> UpdateAsync(Cliente entity)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ACTUALIZAR_CLIENTE, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Cliente", entity.Id_Cliente);
            cmd.Parameters.AddWithValue("@CodigoCliente", entity.CodigoCliente);
            cmd.Parameters.AddWithValue("@P_Nombre", entity.P_Nombre);
            cmd.Parameters.AddWithValue("@S_Nombre", (object?)entity.S_Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@P_Apellido", entity.P_Apellido);
            cmd.Parameters.AddWithValue("@S_Apellido", (object?)entity.S_Apellido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)entity.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Direccion", (object?)entity.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TipoMembresia", (object?)entity.TipoMembresia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", entity.Estado);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", entity.Id_UsuarioModificacion ?? 0);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        public override async Task<bool> DeleteAsync(int id, int userId)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ELIMINAR_CLIENTE, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Cliente", id);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", userId);

            var res = await cmd.ExecuteScalarAsync();
            return res != null;
        }

        /// <summary>
        /// Asigna puntos a un cliente después de una compra usando el procedimiento almacenado
        /// </summary>
        public async Task<(int puntosGanados, string tipoMembresia)> AsignarPuntosClienteAsync(int idCliente, decimal montoTotalCompra, int idUsuarioOperacion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ASIGNAR_PUNTOS_CLIENTE, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id_Cliente", idCliente);
            cmd.Parameters.AddWithValue("@MontoTotalCompra", montoTotalCompra);
            cmd.Parameters.AddWithValue("@Id_UsuarioOperacion", idUsuarioOperacion);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var puntosGanados = reader["PuntosGanadosEnEstaCompra"] is DBNull ? 0 : Convert.ToInt32(reader["PuntosGanadosEnEstaCompra"]);
                var tipoMembresia = reader["Membresia"] is DBNull ? null : reader["Membresia"].ToString();
                return (puntosGanados, tipoMembresia ?? string.Empty);
            }
            return (0, string.Empty);
        }

        private Cliente MapReader(SqlDataReader reader)
        {
            var cliente = new Cliente
            {
                Id_Cliente = Convert.ToInt32(reader["Id_Cliente"]),
                CodigoCliente = reader["CodigoCliente"].ToString() ?? "",
                P_Nombre = reader["P_Nombre"].ToString() ?? "",
                P_Apellido = reader["P_Apellido"].ToString() ?? "",
                Telefono = reader["Telefono"] is DBNull ? null : reader["Telefono"].ToString(),
                Email = reader["Email"] is DBNull ? null : reader["Email"].ToString(),
                TipoMembresia = reader["TipoMembresia"] is DBNull ? null : reader["TipoMembresia"].ToString(),
                Estado = Convert.ToBoolean(reader["Estado"])
            };

            // Campos opcionales que pueden no estar en todos los resultados
            if (HasColumn(reader, "S_Nombre"))
            {
                cliente.S_Nombre = reader["S_Nombre"] is DBNull ? null : reader["S_Nombre"].ToString();
            }

            if (HasColumn(reader, "S_Apellido"))
            {
                cliente.S_Apellido = reader["S_Apellido"] is DBNull ? null : reader["S_Apellido"].ToString();
            }

            if (HasColumn(reader, "Direccion"))
            {
                cliente.Direccion = reader["Direccion"] is DBNull ? null : reader["Direccion"].ToString();
            }

            if (HasColumn(reader, "FechaRegistro"))
            {
                cliente.FechaRegistro = reader["FechaRegistro"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaRegistro"]);
            }

            if (HasColumn(reader, "PuntosCompra"))
            {
                cliente.PuntosCompra = reader["PuntosCompra"] is DBNull ? 0 : Convert.ToInt32(reader["PuntosCompra"]);
            }

            if (HasColumn(reader, "UltimaCompra"))
            {
                cliente.UltimaCompra = reader["UltimaCompra"] is DBNull ? null : Convert.ToDateTime(reader["UltimaCompra"]);
            }

            if (HasColumn(reader, "Id_UsuarioCreacion"))
            {
                cliente.Id_UsuarioCreacion = reader["Id_UsuarioCreacion"] is DBNull ? 0 : Convert.ToInt32(reader["Id_UsuarioCreacion"]);
            }

            if (HasColumn(reader, "Id_UsuarioModificacion"))
            {
                cliente.Id_UsuarioModificacion = reader["Id_UsuarioModificacion"] is DBNull ? null : Convert.ToInt32(reader["Id_UsuarioModificacion"]);
            }

            if (HasColumn(reader, "FechaModificacion"))
            {
                cliente.FechaModificacion = reader["FechaModificacion"] is DBNull ? null : Convert.ToDateTime(reader["FechaModificacion"]);
            }

            return cliente;
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
