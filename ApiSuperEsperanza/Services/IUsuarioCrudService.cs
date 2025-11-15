using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public interface IUsuarioCrudService
    {
        Task<IEnumerable<Usuario>> ListarAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<(bool ok, string error)> CrearAsync(Usuario entity, string clavePlana);
        Task<(bool ok, string error)> ActualizarAsync(Usuario entity, string? clavePlana);
        Task<(bool ok, string error)> EliminarAsync(int id, int idUsuarioModificacion);
    }
}

