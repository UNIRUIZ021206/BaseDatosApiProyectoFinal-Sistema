using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services.Interfaces;

namespace SuperEsperanzaApi.Services
{
    public class ProveedorService : IService<Proveedor>
    {
        private readonly ProveedorDAO _proveedorDAO;

        public ProveedorService(ProveedorDAO proveedorDAO)
        {
            _proveedorDAO = proveedorDAO;
        }

        public async Task<(bool ok, string error)> CrearAsync(Proveedor entity)
        {
            try
            {
                var id = await _proveedorDAO.CreateAsync(entity);
                if (id > 0)
                {
                    entity.Id_Proveedor = id;
                    return (true, string.Empty);
                }
                return (false, "No se pudo crear el proveedor");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<Proveedor>> ListarAsync()
        {
            return await _proveedorDAO.GetAllAsync();
        }

        public async Task<Proveedor?> ObtenerPorIdAsync(int id)
        {
            return await _proveedorDAO.GetByIdAsync(id);
        }

        public async Task<(bool ok, string error)> ActualizarAsync(Proveedor entity)
        {
            try
            {
                var resultado = await _proveedorDAO.UpdateAsync(entity);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar el proveedor");
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
                var resultado = await _proveedorDAO.DeleteAsync(id, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar el proveedor");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

