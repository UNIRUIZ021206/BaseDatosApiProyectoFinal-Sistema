using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
{
    public class LoteCreateDto
    {
        [Required] public string CodigoLote { get; set; } = string.Empty;
        [Required] public int Id_Producto { get; set; }
        [Required] public int Id_Compra { get; set; }
        [Required] public int Cantidad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }

    public class LoteDto
    {
        public int Id_Lote { get; set; }
        public string CodigoLote { get; set; } = string.Empty;
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; }
        public int Id_Compra { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Estado { get; set; }
    }
}

