using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class ReporteRepository : BaseRepository
    {
        public async Task<List<ReporteVentasDto>> GetReporteVentasAsync(DateTime fecha)
        {
            var fechaStr = fecha.ToString("yyyy-MM-dd");
            return await GetListAsync<ReporteVentasDto>($"/api/Reporte/ventas?fecha={fechaStr}");
        }

        public async Task<List<InventarioGeneralDto>> GetInventarioGeneralAsync()
        {
            return await GetListAsync<InventarioGeneralDto>("/api/Reporte/inventario");
        }
    }
}

