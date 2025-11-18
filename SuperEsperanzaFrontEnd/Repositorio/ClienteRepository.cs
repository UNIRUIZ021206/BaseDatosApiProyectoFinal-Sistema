using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class ClienteRepository : BaseRepository
    {
        public async Task<List<ClienteDto>> GetAllAsync()
        {
            return await GetListAsync<ClienteDto>("/api/Cliente");
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            return await GetAsync<ClienteDto>($"/api/Cliente/{id}");
        }

        public async Task<ClienteDto?> CreateAsync(ClienteCreateDto dto)
        {
            return await PostAsync<ClienteDto>("/api/Cliente", dto);
        }

        public async Task<bool> UpdateAsync(int id, ClienteUpdateDto dto)
        {
            var result = await PutAsync<object>($"/api/Cliente/{id}", dto);
            return result != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"/api/Cliente/{id}");
        }
    }
}

