using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Models;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class SesionDAO
    {
        private readonly ConexionDB _conexion;

        public SesionDAO(ConexionDB conexion)
        {
            _conexion = conexion;
        }

        public async Task<IEnumerable<Sesion>> ListarSesionesActivasAsync()
        {
            var lista = new List<Sesion>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_LISTAR_SESIONES_ACTIVAS, cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Sesion
                {
                    Id_Sesion = Convert.ToInt32(reader["Id_Sesion"]),
                    CodigoSesion = reader["CodigoSesion"].ToString() ?? "",
                    UsuarioNombre = reader["UsuarioNombre"].ToString(),
                    FechaApertura = Convert.ToDateTime(reader["FechaApertura"]),
                    MontoInicial = Convert.ToDecimal(reader["MontoInicial"]),
                    Estado = true
                });
            }
            return lista;
        }

        public async Task<int> AbrirSesionAsync(Sesion sesion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_ABRIR_SESION, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CodigoSesion", sesion.CodigoSesion);
            cmd.Parameters.AddWithValue("@Id_Usuario", sesion.Id_Usuario);
            cmd.Parameters.AddWithValue("@MontoInicial", sesion.MontoInicial);
            cmd.Parameters.AddWithValue("@Observacion", (object?)sesion.Observacion ?? DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> CerrarSesionAsync(int idSesion, int idUsuarioModificacion)
        {
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_CERRAR_SESION, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id_Sesion", idSesion);
            cmd.Parameters.AddWithValue("@Id_UsuarioModificacion", idUsuarioModificacion);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}

