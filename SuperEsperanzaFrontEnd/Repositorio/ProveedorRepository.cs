using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class ProveedorRepository : BaseRepository
    {
        public async Task<List<ProveedorDto>> GetAllAsync()
        {
            return await GetListAsync<ProveedorDto>("/api/Proveedor");
        }

        public async Task<ProveedorDto?> GetByIdAsync(int id)
        {
            return await GetAsync<ProveedorDto>($"/api/Proveedor/{id}");
        }

        public async Task<ProveedorDto?> CreateAsync(ProveedorCreateDto dto)
        {
            return await PostAsync<ProveedorDto>("/api/Proveedor", dto);
        }

        public async Task<bool> UpdateAsync(int id, ProveedorUpdateDto dto)
        {
            var result = await PutAsync<object>($"/api/Proveedor/{id}", dto);
            return result != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"/api/Proveedor/{id}");
        }
    }
}

