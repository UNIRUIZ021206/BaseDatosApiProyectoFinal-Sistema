using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public class UsuarioCrudService : IUsuarioCrudService
    {
        private readonly UsuarioDAO _usuarioDAO;

        public UsuarioCrudService(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public async Task<IEnumerable<Usuario>> ListarAsync()
        {
            return await _usuarioDAO.ListarUsuariosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _usuarioDAO.ObtenerUsuarioPorIdAsync(id);
        }

        public async Task<(bool ok, string error)> CrearAsync(Usuario entity, string clavePlana)
        {
            try
            {
                var id = await _usuarioDAO.InsertarUsuarioAsync(entity, clavePlana, entity.Id_UsuarioCreacion ?? 0);
                if (id > 0)
                {
                    entity.Id_Usuario = id;
                    entity.Id = id; // Compatibilidad
                    return (true, string.Empty);
                }
                return (false, "No se pudo crear el usuario");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> ActualizarAsync(Usuario entity, string? clavePlana)
        {
            try
            {
                var resultado = await _usuarioDAO.ActualizarUsuarioAsync(entity, clavePlana, entity.Id_UsuarioModificacion ?? 0);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar el usuario");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> EliminarAsync(int id, int idUsuarioModificacion)
        {
            try
            {
                var resultado = await _usuarioDAO.EliminarUsuarioAsync(id, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar el usuario");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

