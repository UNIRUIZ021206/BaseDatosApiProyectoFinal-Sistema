using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class RolRepository : BaseRepository
    {
        public async Task<List<RolDto>> GetAllAsync()
        {
            return await GetListAsync<RolDto>("/api/Rol");
        }

        public async Task<RolDto?> GetByIdAsync(int id)
        {
            return await GetAsync<RolDto>($"/api/Rol/{id}");
        }
    }
}

