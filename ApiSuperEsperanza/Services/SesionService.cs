using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public interface ISesionService
    {
        Task<IEnumerable<Sesion>> ListarSesionesActivasAsync();
        Task<(bool ok, string error, int? idSesion)> AbrirSesionAsync(Sesion sesion);
        Task<(bool ok, string error)> CerrarSesionAsync(int idSesion, int idUsuarioModificacion);
    }

    public class SesionService : ISesionService
    {
        private readonly SesionDAO _sesionDAO;

        public SesionService(SesionDAO sesionDAO)
        {
            _sesionDAO = sesionDAO;
        }

        public async Task<IEnumerable<Sesion>> ListarSesionesActivasAsync()
        {
            return await _sesionDAO.ListarSesionesActivasAsync();
        }

        public async Task<(bool ok, string error, int? idSesion)> AbrirSesionAsync(Sesion sesion)
        {
            try
            {
                var idSesion = await _sesionDAO.AbrirSesionAsync(sesion);
                return (true, string.Empty, idSesion);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool ok, string error)> CerrarSesionAsync(int idSesion, int idUsuarioModificacion)
        {
            try
            {
                var resultado = await _sesionDAO.CerrarSesionAsync(idSesion, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo cerrar la sesión");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

