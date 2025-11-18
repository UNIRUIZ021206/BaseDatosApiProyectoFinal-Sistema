using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
{
    public class FacturaCreateDto
    {
        [Required] public string CodigoFactura { get; set; } = string.Empty;
        // Id_Cliente puede ser null para cliente general, por lo que no usamos [Required]
        public int? Id_Cliente { get; set; }
        [Required] public int Id_Sesion { get; set; }
        [Required] public string NumeroFactura { get; set; } = string.Empty;
        [Required] public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        [Required] public List<DetalleFacturaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleFacturaCreateDto
    {
        [Required] public int Id_Lote { get; set; }
        [Required] public int Cantidad { get; set; }
        [Required] public decimal PrecioUnitario { get; set; }
    }

    public class FacturaDto
    {
        public int Id_Factura { get; set; }
        public string CodigoFactura { get; set; } = string.Empty;
        public int? Id_Cliente { get; set; }
        public string? ClienteNombre { get; set; }
        public int Id_Sesion { get; set; }
        public string? CodigoSesion { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public string EstadoFactura { get; set; } = string.Empty;
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
        public List<DetalleFacturaDto>? Detalles { get; set; }
    }

    public class DetalleFacturaDto
    {
        public int Id_DetalleFactura { get; set; }
        public int Id_Lote { get; set; }
        public string? CodigoLote { get; set; }
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class FacturaAnularDto
    {
        [Required] public int Id_Factura { get; set; }
        [Required] public string Motivo { get; set; } = string.Empty;
    }
}

