using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services.Interfaces;

namespace SuperEsperanzaApi.Services
{
    public class CategoriaService : IService<Categoria>
    {
        private readonly CategoriaDAO _categoriaDAO;

        public CategoriaService(CategoriaDAO categoriaDAO)
        {
            _categoriaDAO = categoriaDAO;
        }

        public async Task<(bool ok, string error)> CrearAsync(Categoria entity)
        {
            try
            {
                var id = await _categoriaDAO.CreateAsync(entity);
                if (id > 0)
                {
                    entity.Id_Categoria = id;
                    return (true, string.Empty);
                }
                return (false, "No se pudo crear la categoría");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            return await _categoriaDAO.GetAllAsync();
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            return await _categoriaDAO.GetByIdAsync(id);
        }

        public async Task<(bool ok, string error)> ActualizarAsync(Categoria entity)
        {
            try
            {
                var resultado = await _categoriaDAO.UpdateAsync(entity);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar la categoría");
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
                var resultado = await _categoriaDAO.DeleteAsync(id, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar la categoría");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
