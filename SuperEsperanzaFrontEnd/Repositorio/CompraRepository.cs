using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class CompraRepository : BaseRepository
    {
        public async Task<CompraDto?> CreateAsync(CompraCreateDto dto)
        {
            return await PostAsync<CompraDto>("/api/Compra", dto);
        }
    }
}

