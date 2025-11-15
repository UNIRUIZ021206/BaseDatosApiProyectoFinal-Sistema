namespace SuperEsperanzaApi.Models
{
    public class Sesion
    {
        public int Id_Sesion { get; set; }
        public string CodigoSesion { get; set; } = string.Empty;
        public int Id_Usuario { get; set; }
        public string? UsuarioNombre { get; set; } // Para JOIN
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal? MontoFinal { get; set; }
        public bool Estado { get; set; } // 1 = Abierta, 0 = Cerrada
        public string? Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int Id_UsuarioCreacion { get; set; }
        public int? Id_UsuarioModificacion { get; set; }
    }
}

