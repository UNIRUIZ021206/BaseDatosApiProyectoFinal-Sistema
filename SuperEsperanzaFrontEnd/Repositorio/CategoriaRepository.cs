using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class CategoriaRepository : BaseRepository
    {
        public async Task<List<CategoriaDto>> GetAllAsync()
        {
            return await GetListAsync<CategoriaDto>("/api/Categoria");
        }

        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            return await GetAsync<CategoriaDto>($"/api/Categoria/{id}");
        }

        public async Task<CategoriaDto?> CreateAsync(CategoriaCreateDto dto)
        {
            return await PostAsync<CategoriaDto>("/api/Categoria", dto);
        }

        public async Task<bool> UpdateAsync(int id, CategoriaUpdateDto dto)
        {
            var result = await PutAsync<object>($"/api/Categoria/{id}", dto);
            return result != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"/api/Categoria/{id}");
        }
    }
}

