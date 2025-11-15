namespace SuperEsperanzaApi.Models
{
    public class Factura
    {
        public int Id_Factura { get; set; }
        public string CodigoFactura { get; set; } = string.Empty;
        public int Id_Cliente { get; set; }
        public string? ClienteNombre { get; set; } // Para JOIN
        public int Id_Sesion { get; set; }
        public string? CodigoSesion { get; set; } // Para JOIN
        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; } // Calculado: Subtotal - Descuento + Impuesto
        public string EstadoFactura { get; set; } = "ACTIVA"; // ACTIVA, PROCESADA, ANULADA
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
        public List<DetalleFactura>? Detalles { get; set; }
    }

    public class DetalleFactura
    {
        public int Id_DetalleFactura { get; set; }
        public int Id_Factura { get; set; }
        public int Id_Lote { get; set; }
        public string? CodigoLote { get; set; } // Para JOIN
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; } // Para JOIN
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; } // Calculado: Cantidad * PrecioUnitario
        public DateTime FechaCreacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
    }
}

