using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services.Interfaces;

namespace SuperEsperanzaApi.Services
{
    public class ProductoService : IService<Producto>
    {
        private readonly ProductoDAO _productoDAO;

        public ProductoService(ProductoDAO productoDAO)
        {
            _productoDAO = productoDAO;
        }

        public async Task<(bool ok, string error)> CrearAsync(Producto entity)
        {
            try
            {
                var id = await _productoDAO.CreateAsync(entity);
                if (id > 0)
                {
                    entity.Id_Producto = id;
                    return (true, string.Empty);
                }
                return (false, "No se pudo crear el producto");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<Producto>> ListarAsync()
        {
            return await _productoDAO.GetAllAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _productoDAO.GetByIdAsync(id);
        }

        public async Task<(bool ok, string error)> ActualizarAsync(Producto entity)
        {
            try
            {
                var resultado = await _productoDAO.UpdateAsync(entity);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar el producto");
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
                var resultado = await _productoDAO.DeleteAsync(id, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar el producto");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

