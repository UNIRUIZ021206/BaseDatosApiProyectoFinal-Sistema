using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class UsuarioRepository : BaseRepository
    {
        public async Task<List<UsuarioDto>> GetAllAsync()
        {
            return await GetListAsync<UsuarioDto>("/api/Usuario");
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            return await GetAsync<UsuarioDto>($"/api/Usuario/{id}");
        }

        public async Task<UsuarioDto?> CreateAsync(UsuarioCreateDto dto)
        {
            return await PostAsync<UsuarioDto>("/api/Usuario", dto);
        }

        public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
        {
            var result = await PutAsync<object>($"/api/Usuario/{id}", dto);
            return result != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync($"/api/Usuario/{id}");
        }
    }
}

