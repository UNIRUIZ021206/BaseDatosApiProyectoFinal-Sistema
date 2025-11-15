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
                // Calcular subtotales en los detalles
                if (compra.Detalles != null)
                {
                    foreach (var detalle in compra.Detalles)
                    {
                        detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    }
                }

                // Insertar la compra con sus detalles
                var idCompra = await _compraDAO.InsertarCompraAsync(compra);
                compra.Id_Compra = idCompra;

                // Crear lotes para cada detalle de compra
                if (compra.Detalles != null)
                {
                    foreach (var detalle in compra.Detalles)
                    {
                        var lote = new Lote
                        {
                            CodigoLote = $"LOTE-{compra.CodigoCompra}-{detalle.Id_Producto}",
                            Id_Producto = detalle.Id_Producto,
                            Id_Compra = idCompra,
                            Cantidad = detalle.Cantidad,
                            FechaVencimiento = null, // Se puede agregar lógica para calcular esto
                            Id_UsuarioCreacion = compra.Id_UsuarioCreacion
                        };

                        await _loteDAO.InsertarLoteAsync(lote);
                    }
                }

                return (true, string.Empty, idCompra);
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

