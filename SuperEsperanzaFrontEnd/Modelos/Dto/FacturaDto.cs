namespace SuperEsperanzaFrontEnd.Modelos.Dto
{
    public class FacturaCreateDto
    {
        public string CodigoFactura { get; set; } = string.Empty;
        public int? Id_Cliente { get; set; }
        public int Id_Sesion { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public List<DetalleFacturaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleFacturaCreateDto
    {
        public int Id_Lote { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
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
        public int Id_Factura { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}

