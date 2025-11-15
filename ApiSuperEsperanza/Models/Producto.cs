namespace SuperEsperanzaApi.Models
{
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public int Id_Categoria { get; set; }
        public string? NombreCategoria { get; set; } // Para el JOIN con Categoria
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public decimal CostoPromedio { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
    }
}

