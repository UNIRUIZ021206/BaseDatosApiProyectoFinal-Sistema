using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class LoteRepository : BaseRepository
    {
        public async Task<List<LoteDto>> GetAllAsync()
        {
            return await GetListAsync<LoteDto>("/api/Lote");
        }

        public async Task<LoteDto?> GetByIdAsync(int id)
        {
            return await GetAsync<LoteDto>($"/api/Lote/{id}");
        }

        // Obtener lotes disponibles (con stock) para un producto
        public async Task<List<LoteDto>> GetLotesDisponiblesPorProductoAsync(int idProducto)
        {
            try
            {
                // Usar el endpoint específico para obtener lotes por producto
                var lotes = await GetListAsync<LoteDto>($"/api/Lote/producto/{idProducto}");
                // Filtrar solo los que tienen stock disponible
                return lotes
                    .Where(l => l.Estado && l.Cantidad > 0)
                    .OrderBy(l => l.FechaVencimiento ?? DateTime.MaxValue)
                    .ToList();
            }
            catch
            {
                // Si falla, intentar obtener todos y filtrar
                var todosLotes = await GetAllAsync();
                return todosLotes
                    .Where(l => l.Id_Producto == idProducto && l.Estado && l.Cantidad > 0)
                    .OrderBy(l => l.FechaVencimiento ?? DateTime.MaxValue)
                    .ToList();
            }
        }

        public async Task<LoteDto?> CreateAsync(LoteCreateDto dto)
        {
            return await PostAsync<LoteDto>("/api/Lote", dto);
        }
    }
}

