using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Dto;

namespace SuperEsperanzaApi.Services
{
    public interface IReporteService
    {
        Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime fecha);
        Task<List<InventarioGeneralDto>> ObtenerInventarioGeneralAsync();
    }

    public class ReporteService : IReporteService
    {
        private readonly ReporteDAO _reporteDAO;

        public ReporteService(ReporteDAO reporteDAO)
        {
            _reporteDAO = reporteDAO;
        }

        public async Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime fecha)
        {
            return await _reporteDAO.ObtenerReporteVentasAsync(fecha);
        }

        public async Task<List<InventarioGeneralDto>> ObtenerInventarioGeneralAsync()
        {
            return await _reporteDAO.ObtenerInventarioGeneralAsync();
        }
    }
}

