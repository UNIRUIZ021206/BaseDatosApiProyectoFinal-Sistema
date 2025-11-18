namespace SuperEsperanzaApi.Dto
{
    public class ReporteVentasDto
    {
        public string Producto { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVenta { get; set; }
    }

    public class InventarioGeneralDto
    {
        public int Id_Producto { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public int StockDisponibleEnLotes { get; set; }
        public bool EstadoProducto { get; set; }
    }
}

