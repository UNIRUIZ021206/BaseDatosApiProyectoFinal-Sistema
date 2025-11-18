using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class SesionRepository : BaseRepository
    {
        public async Task<List<SesionDto>> GetActivasAsync()
        {
            return await GetListAsync<SesionDto>("/api/Sesion/activas");
        }

        public async Task<SesionDto?> AbrirAsync(SesionCreateDto dto)
        {
            return await PostAsync<SesionDto>("/api/Sesion/abrir", dto);
        }

        public async Task<bool> CerrarAsync(int idSesion)
        {
            var dto = new SesionCerrarDto { Id_Sesion = idSesion };
            var result = await PostAsync<object>("/api/Sesion/cerrar", dto);
            return result != null;
        }
    }
}

