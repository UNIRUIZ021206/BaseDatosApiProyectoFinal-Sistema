using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public interface ILoteService
    {
        Task<Lote?> ObtenerLotePorIdAsync(int idLote);
        Task<(bool ok, string error, int? idLote)> CrearLoteAsync(Lote lote);
        Task<IEnumerable<Lote>> ListarLotesPorProductoAsync(int idProducto);
    }

    public class LoteService : ILoteService
    {
        private readonly LoteDAO _loteDAO;

        public LoteService(LoteDAO loteDAO)
        {
            _loteDAO = loteDAO;
        }

        public async Task<Lote?> ObtenerLotePorIdAsync(int idLote)
        {
            return await _loteDAO.ObtenerLotePorIdAsync(idLote);
        }

        public async Task<(bool ok, string error, int? idLote)> CrearLoteAsync(Lote lote)
        {
            try
            {
                var idLote = await _loteDAO.InsertarLoteAsync(lote);
                return (true, string.Empty, idLote);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<IEnumerable<Lote>> ListarLotesPorProductoAsync(int idProducto)
        {
            return await _loteDAO.ListarLotesPorProductoAsync(idProducto);
        }
    }
}

