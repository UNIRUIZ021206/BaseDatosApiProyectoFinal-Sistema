namespace SuperEsperanzaApi.Models
{
    public class Compra
    {
        public int Id_Compra { get; set; }
        public string CodigoCompra { get; set; } = string.Empty;
        public int Id_Proveedor { get; set; }
        public string? NombreProveedor { get; set; } // Para JOIN
        public DateTime FechaCompra { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public List<DetalleCompra>? Detalles { get; set; }
    }

    public class DetalleCompra
    {
        public int Id_DetalleCompra { get; set; }
        public int Id_Compra { get; set; }
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; } // Para JOIN
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; } // Calculado: Cantidad * PrecioUnitario
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
    }
}

