using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class ProductoRepository : BaseRepository
    {
        public async Task<List<ProductoDto>> GetAllAsync()
        {
            return await GetListAsync<ProductoDto>("/api/Producto");
        }

        public async Task<ProductoDto?> GetByIdAsync(int id)
        {
            return await GetAsync<ProductoDto>($"/api/Producto/{id}");
        }

        public async Task<ProductoDto?> CreateAsync(ProductoCreateDto dto)
        {
            return await PostAsync<ProductoDto>("/api/Producto", dto);
        }

        public async Task<bool> UpdateAsync(int id, ProductoUpdateDto dto)
        {
            var result = await PutAsync<object>($"/api/Producto/{id}", dto);
            return result != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"/api/Producto/{id}");
        }
    }
}

