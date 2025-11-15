using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services.Interfaces;

namespace SuperEsperanzaApi.Services
{
    public class ClienteService : IService<Cliente>
    {
        private readonly ClienteDAO _clienteDAO;

        public ClienteService(ClienteDAO clienteDAO)
        {
            _clienteDAO = clienteDAO;
        }

        public async Task<(bool ok, string error)> CrearAsync(Cliente entity)
        {
            try
            {
                var id = await _clienteDAO.CreateAsync(entity);
                if (id > 0)
                {
                    entity.Id_Cliente = id;
                    return (true, string.Empty);
                }
                return (false, "No se pudo crear el cliente");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<Cliente>> ListarAsync()
        {
            return await _clienteDAO.GetAllAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _clienteDAO.GetByIdAsync(id);
        }

        public async Task<(bool ok, string error)> ActualizarAsync(Cliente entity)
        {
            try
            {
                var resultado = await _clienteDAO.UpdateAsync(entity);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar el cliente");
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
                var resultado = await _clienteDAO.DeleteAsync(id, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar el cliente");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

