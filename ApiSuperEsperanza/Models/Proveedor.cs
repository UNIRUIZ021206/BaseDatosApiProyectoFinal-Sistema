namespace SuperEsperanzaApi.Models
{
    public class Proveedor
    {
        public int Id_Proveedor { get; set; }
        public string CodigoProveedor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
    }
}

