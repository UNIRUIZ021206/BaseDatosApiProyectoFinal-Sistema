using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
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
        [Required] public string CodigoProducto { get; set; } = string.Empty;
        [Required] public int Id_Categoria { get; set; }
        [Required] public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        [Required] public decimal PrecioVenta { get; set; }
        [Required] public int StockActual { get; set; }
    }

    public class ProductoUpdateDto
    {
        [Required] public string CodigoProducto { get; set; } = string.Empty;
        [Required] public int Id_Categoria { get; set; }
        [Required] public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        [Required] public decimal PrecioVenta { get; set; }
        [Required] public int StockActual { get; set; }
        [Required] public bool Estado { get; set; }
    }
}

