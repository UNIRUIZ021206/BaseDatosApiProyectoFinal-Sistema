namespace SuperEsperanzaApi.Models
{
    public class Cliente
    {
        public int Id_Cliente { get; set; }
        public string CodigoCliente { get; set; } = string.Empty;
        public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaCompra { get; set; }
        public int PuntosCompra { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
    }
}

