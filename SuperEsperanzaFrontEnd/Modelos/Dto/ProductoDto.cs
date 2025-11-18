namespace SuperEsperanzaFrontEnd.Modelos.Dto
{
    public class ProductoDto
    {
        public int Id_Producto { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public int Id_Categoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public bool Estado { get; set; }
    }

    public class ProductoCreateDto
    {
        public string CodigoProducto { get; set; } = string.Empty;
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
    }

    public class ProductoUpdateDto
    {
        public string CodigoProducto { get; set; } = string.Empty;
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public bool Estado { get; set; }
    }
}

