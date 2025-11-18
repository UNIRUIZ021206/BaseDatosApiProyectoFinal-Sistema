using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class FacturaRepository : BaseRepository
    {
        public async Task<FacturaDto?> CreateAsync(FacturaCreateDto dto)
        {
            return await PostAsync<FacturaDto>("/api/Factura", dto);
        }

        public async Task<List<DetalleFacturaDto>> GetDetallesAsync(int idFactura)
        {
            return await GetListAsync<DetalleFacturaDto>($"/api/Factura/{idFactura}/detalles");
        }

        public async Task<bool> AnularAsync(FacturaAnularDto dto)
        {
            var result = await PostAsync<object>("/api/Factura/anular", dto);
            return result != null;
        }
    }
}

