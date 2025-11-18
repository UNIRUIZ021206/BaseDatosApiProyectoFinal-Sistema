namespace SuperEsperanzaFrontEnd.Modelos.Dto
{
    public class CompraCreateDto
    {
        public string CodigoCompra { get; set; } = string.Empty;
        public int Id_Proveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<DetalleCompraCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleCompraCreateDto
    {
        public int Id_Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class CompraDto
    {
        public int Id_Compra { get; set; }
        public string CodigoCompra { get; set; } = string.Empty;
        public int Id_Proveedor { get; set; }
        public string? NombreProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<DetalleCompraDto>? Detalles { get; set; }
    }

    public class DetalleCompraDto
    {
        public int Id_DetalleCompra { get; set; }
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}

