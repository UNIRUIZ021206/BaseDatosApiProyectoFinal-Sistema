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
        private readonly ClienteDAO _clienteDAO;

        public FacturaService(FacturaDAO facturaDAO, ClienteDAO clienteDAO)
        {
            _facturaDAO = facturaDAO;
            _clienteDAO = clienteDAO;
        }

        public async Task<(bool ok, string error, int? idFactura)> CrearFacturaAsync(Factura factura)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[FacturaService.CrearFacturaAsync] Iniciando creación de factura");
                System.Diagnostics.Debug.WriteLine($"  - Usuario Creación: {factura.Id_UsuarioCreacion}");
                System.Diagnostics.Debug.WriteLine($"  - Cantidad de detalles: {factura.Detalles?.Count ?? 0}");
                
                // Validar que haya detalles
                if (factura.Detalles == null || !factura.Detalles.Any())
                {
                    return (false, "La factura debe tener al menos un detalle.", null);
                }

                // Validar detalles antes de procesar
                System.Diagnostics.Debug.WriteLine($"[FacturaService.CrearFacturaAsync] Validando detalles...");
                    foreach (var detalle in factura.Detalles)
                    {
                    System.Diagnostics.Debug.WriteLine($"  Validando detalle: Lote ID {detalle.Id_Lote}, Cantidad {detalle.Cantidad}, Precio {detalle.PrecioUnitario}");
                    
                    // Validar que la cantidad sea positiva
                    if (detalle.Cantidad <= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"  ERROR: Cantidad inválida para lote ID {detalle.Id_Lote}");
                        return (false, $"La cantidad debe ser mayor a cero para el lote ID {detalle.Id_Lote}.", null);
                    }

                    // Validar que el precio sea positivo
                    if (detalle.PrecioUnitario <= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"  ERROR: Precio inválido para lote ID {detalle.Id_Lote}");
                        return (false, $"El precio unitario debe ser mayor a cero para el lote ID {detalle.Id_Lote}.", null);
                    }

                    // Calcular subtotal
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    System.Diagnostics.Debug.WriteLine($"  Subtotal calculado: {detalle.Subtotal}");
                }

                // Calcular el total de la factura
                factura.Total = factura.Subtotal - factura.Descuento + factura.Impuesto;
                System.Diagnostics.Debug.WriteLine($"[FacturaService.CrearFacturaAsync] Total calculado: {factura.Total}");

                System.Diagnostics.Debug.WriteLine($"[FacturaService.CrearFacturaAsync] Llamando a InsertarFacturaAsync con {factura.Detalles.Count} detalles");
                // Insertar la factura con sus detalles
                var idFactura = await _facturaDAO.InsertarFacturaAsync(factura);
                System.Diagnostics.Debug.WriteLine($"[FacturaService.CrearFacturaAsync] Factura insertada con ID: {idFactura}");
                factura.Id_Factura = idFactura;

                // Asignar puntos al cliente si tiene un cliente asociado (no es cliente general)
                if (factura.Id_Cliente.HasValue && factura.Id_Cliente.Value > 0)
                {
                    try
                    {
                        var (puntosGanados, tipoMembresia) = await _clienteDAO.AsignarPuntosClienteAsync(
                            factura.Id_Cliente.Value,
                            factura.Total,
                            factura.Id_UsuarioCreacion
                        );
                        // Los puntos se asignan automáticamente por el procedimiento almacenado
                    }
                    catch (Exception ex)
                    {
                        // Si falla la asignación de puntos, no falla la factura
                        // Solo registramos el error pero continuamos
                        System.Diagnostics.Debug.WriteLine($"Error al asignar puntos: {ex.Message}");
                    }
                }

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

