using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Services
{
    public interface IFacturaService
    {
        Task<(bool ok, string error, int? idFactura)> CrearFacturaAsync(Factura factura);
        Task<IEnumerable<DetalleFactura>> ListarDetallesPorFacturaAsync(int idFactura);
        Task<(bool ok, string error)> AnularFacturaAsync(int idFactura, string motivo, int idUsuarioModificacion);
    }

    public class FacturaService : IFacturaService
    {
        private readonly FacturaDAO _facturaDAO;

        public FacturaService(FacturaDAO facturaDAO)
        {
            _facturaDAO = facturaDAO;
        }

        public async Task<(bool ok, string error, int? idFactura)> CrearFacturaAsync(Factura factura)
        {
            try
            {
                // Calcular subtotales en los detalles
                if (factura.Detalles != null)
                {
                    foreach (var detalle in factura.Detalles)
                    {
                        detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    }
                }

                // Calcular el total de la factura
                factura.Total = factura.Subtotal - factura.Descuento + factura.Impuesto;

                // Insertar la factura con sus detalles
                var idFactura = await _facturaDAO.InsertarFacturaAsync(factura);
                factura.Id_Factura = idFactura;

                return (true, string.Empty, idFactura);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<IEnumerable<DetalleFactura>> ListarDetallesPorFacturaAsync(int idFactura)
        {
            return await _facturaDAO.ListarDetallesPorFacturaAsync(idFactura);
        }

        public async Task<(bool ok, string error)> AnularFacturaAsync(int idFactura, string motivo, int idUsuarioModificacion)
        {
            try
            {
                var resultado = await _facturaDAO.AnularFacturaAsync(idFactura, motivo, idUsuarioModificacion);
                return resultado ? (true, string.Empty) : (false, "La factura no existe o ya está anulada");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

