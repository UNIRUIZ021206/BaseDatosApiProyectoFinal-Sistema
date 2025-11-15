namespace SuperEsperanzaApi.Models
{
    public class Lote
    {
        public int Id_Lote { get; set; }
        public string CodigoLote { get; set; } = string.Empty;
        public int Id_Producto { get; set; }
        public string? NombreProducto { get; set; } // Para JOIN
        public int Id_Compra { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Estado { get; set; } // 1 = Disponible, 0 = Agotado/Vencido
        public DateTime FechaCreacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
    }
}

