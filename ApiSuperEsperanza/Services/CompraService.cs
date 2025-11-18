using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public interface ICompraService
    {
        Task<(bool ok, string error, int? idCompra)> CrearCompraAsync(Compra compra);
        Task<IEnumerable<DetalleCompra>> ListarDetallesPorCompraAsync(int idCompra);
        Task<(bool ok, string error)> ActualizarDetalleCompraAsync(DetalleCompra detalle, int idUsuarioModificacion);
        Task<(bool ok, string error)> EliminarDetalleCompraAsync(int idDetalleCompra);
    }

    public class CompraService : ICompraService
    {
        private readonly CompraDAO _compraDAO;
        private readonly LoteDAO _loteDAO;

        public CompraService(CompraDAO compraDAO, LoteDAO loteDAO)
        {
            _compraDAO = compraDAO;
            _loteDAO = loteDAO;
        }

        public async Task<(bool ok, string error, int? idCompra)> CrearCompraAsync(Compra compra)
        {
            try
            {
                // Validar que haya detalles
                if (compra.Detalles == null || !compra.Detalles.Any())
                {
                    return (false, "La compra debe tener al menos un detalle.", null);
                }

                // Validar detalles antes de procesar
                foreach (var detalle in compra.Detalles)
                {
                    // Validar que la cantidad sea positiva
                    if (detalle.Cantidad <= 0)
                    {
                        return (false, $"La cantidad debe ser mayor a cero para el producto ID {detalle.Id_Producto}.", null);
                    }

                    // Validar que el precio sea positivo
                    if (detalle.PrecioUnitario <= 0)
                    {
                        return (false, $"El precio unitario debe ser mayor a cero para el producto ID {detalle.Id_Producto}.", null);
                    }

                    // Calcular subtotal
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                }

                // Insertar la compra con sus detalles y lotes (todo dentro de una transacción en CompraDAO)
                // Los lotes se crean automáticamente dentro de la transacción de CompraDAO
                var idCompra = await _compraDAO.InsertarCompraAsync(compra);
                compra.Id_Compra = idCompra;

                return (true, string.Empty, idCompra);
            }
            catch (ArgumentException ex)
            {
                // Capturar errores de validación
                return (false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<IEnumerable<DetalleCompra>> ListarDetallesPorCompraAsync(int idCompra)
        {
            return await _compraDAO.ListarDetallesPorCompraAsync(idCompra);
        }

        public async Task<(bool ok, string error)> ActualizarDetalleCompraAsync(DetalleCompra detalle, int idUsuarioModificacion)
        {
            try
            {
                // Recalcular subtotal
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;

                var resultado = await _compraDAO.ActualizarDetalleCompraAsync(detalle, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "No se pudo actualizar el detalle de compra");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool ok, string error)> EliminarDetalleCompraAsync(int idDetalleCompra)
        {
            try
            {
                var resultado = await _compraDAO.EliminarDetalleCompraAsync(idDetalleCompra);
                return resultado ? (true, string.Empty) : (false, "No se pudo eliminar el detalle de compra");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
