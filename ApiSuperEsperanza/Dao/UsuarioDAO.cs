using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;


namespace SuperEsperanzaApi.Dao
{
    public class UsuarioDAO
    {
        private readonly ConexionDB _conexion;
        private readonly ILogger<UsuarioDAO> _logger;
        
        public UsuarioDAO(ConexionDB conexion, ILogger<UsuarioDAO> logger)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<Usuario?> ValidarUsuarioAsync(string codigoUsuario, string contrasena)
        {
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_VALIDAR_USUARIO, conn)
                { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.NVarChar) { Value = codigoUsuario ?? (object)DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Contrasena", SqlDbType.NVarChar) { Value = contrasena ?? (object)DBNull.Value });

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Usuario
                    {
                        Id = Convert.ToInt32(reader["IdUsuario"]),
                        NombreUsuario = reader["NombreUsuario"].ToString() ?? string.Empty,
                        Rol = reader["NombreRol"].ToString() ?? string.Empty
                    };
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de SQL al validar usuario. Número: {Number}, Mensaje: {Message}", ex.Number, ex.Message);
                throw new InvalidOperationException($"Error de base de datos al validar usuario: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al validar usuario. Tipo: {Type}, Mensaje: {Message}", ex.GetType().Name, ex.Message);
                throw new InvalidOperationException($"Error inesperado al validar usuario: {ex.Message}", ex);
            }
        }

        // Métodos CRUD
        public async Task<IEnumerable<Usuario>> ListarUsuariosAsync()
        {
            var resultados = new List<Usuario>();
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_USUARIOS, conn) { CommandType = CommandType.StoredProcedure };

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    resultados.Add(new Usuario
                    {
                        Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]),
                        Id = Convert.ToInt32(reader["Id_Usuario"]), // Compatibilidad
                        CodigoUsuario = reader["CodigoUsuario"].ToString() ?? string.Empty,
                        Nombre = reader["Nombre"].ToString() ?? string.Empty,
                        Apellido = reader["Apellido"].ToString() ?? string.Empty,
                        Email = reader["Email"] is DBNull ? null : reader["Email"].ToString(),
                        Estado = Convert.ToBoolean(reader["Estado"]),
                        Rol = reader["NombreRol"].ToString() ?? string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar usuarios");
                throw;
            }

            return resultados;
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_OBTENER_USUARIO_POR_ID, conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int) { Value = id });

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Usuario
                    {
                        Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]),
                        Id = Convert.ToInt32(reader["Id_Usuario"]), // Compatibilidad
                        CodigoUsuario = reader["CodigoUsuario"].ToString() ?? string.Empty,
                        Nombre = reader["Nombre"].ToString() ?? string.Empty,
                        Apellido = reader["Apellido"].ToString() ?? string.Empty,
                        Email = reader["Email"] is DBNull ? null : reader["Email"].ToString(),
                        Telefono = reader["Telefono"] is DBNull ? null : reader["Telefono"].ToString(),
                        Id_Rol = Convert.ToInt32(reader["Id_Rol"]),
                        IdRol = Convert.ToInt32(reader["Id_Rol"]), // Compatibilidad
                        Estado = Convert.ToBoolean(reader["Estado"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por id {Id}", id);
                throw;
            }
        }

        public async Task<int> InsertarUsuarioAsync(Usuario usuario, string clavePlana, int idUsuarioCreacion)
        {
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_INSERTAR_USUARIO, conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.VarChar, 10) { Value = usuario.CodigoUsuario });
                cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar, 50) { Value = usuario.Nombre });
                cmd.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar, 50) { Value = usuario.Apellido });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = (object?)usuario.Email ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar, 15) { Value = (object?)usuario.Telefono ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Clave", SqlDbType.VarChar, 100) { Value = clavePlana });
                cmd.Parameters.Add(new SqlParameter("@Id_Rol", SqlDbType.Int) { Value = usuario.Id_Rol });
                cmd.Parameters.Add(new SqlParameter("@Id_UsuarioCreacion", SqlDbType.Int) { Value = idUsuarioCreacion });

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar usuario");
                throw;
            }
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario, string? clavePlana, int idUsuarioModificacion)
        {
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_ACTUALIZAR_USUARIO, conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int) { Value = usuario.Id_Usuario });
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.VarChar, 10) { Value = usuario.CodigoUsuario });
                cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar, 50) { Value = usuario.Nombre });
                cmd.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar, 50) { Value = usuario.Apellido });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = (object?)usuario.Email ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar, 15) { Value = (object?)usuario.Telefono ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Clave", SqlDbType.VarChar, 100) { Value = (object?)clavePlana ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Id_Rol", SqlDbType.Int) { Value = usuario.Id_Rol });
                cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Bit) { Value = usuario.Estado });
                cmd.Parameters.Add(new SqlParameter("@Id_UsuarioModificacion", SqlDbType.Int) { Value = idUsuarioModificacion });

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario {Id}", usuario.Id_Usuario);
                throw;
            }
        }

        public async Task<bool> EliminarUsuarioAsync(int idUsuario, int idUsuarioModificacion)
        {
            try
            {
                using var conn = _conexion.ObtenerConexion();
                await conn.OpenAsync();
                using var cmd = new SqlCommand(Procedimientos.SP_ELIMINAR_USUARIO, conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int) { Value = idUsuario });
                cmd.Parameters.Add(new SqlParameter("@Id_UsuarioModificacion", SqlDbType.Int) { Value = idUsuarioModificacion });

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario {Id}", idUsuario);
                throw;
            }
        }
    }
}