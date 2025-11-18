namespace SuperEsperanzaFrontEnd.Modelos.Dto
{
    public class SesionDto
    {
        public int Id_Sesion { get; set; }
        public string CodigoSesion { get; set; } = string.Empty;
        public int Id_Usuario { get; set; }
        public string? UsuarioNombre { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal? MontoFinal { get; set; }
        public bool Estado { get; set; }
        public string? Observacion { get; set; }
    }

    public class SesionCreateDto
    {
        public string CodigoSesion { get; set; } = string.Empty;
        public decimal MontoInicial { get; set; }
        public string? Observacion { get; set; }
    }

    public class SesionCerrarDto
    {
        public int Id_Sesion { get; set; }
    }
}

