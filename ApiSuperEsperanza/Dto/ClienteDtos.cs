using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
{
    public class ClienteDto
    {
        public int Id_Cliente { get; set; }
        public string CodigoCliente { get; set; } = string.Empty;
        public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
        public int PuntosCompra { get; set; }
        public bool Estado { get; set; }
    }

    public class ClienteCreateDto
    {
        [Required] public string CodigoCliente { get; set; } = string.Empty;
        [Required] public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        [Required] public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
    }

    public class ClienteUpdateDto
    {
        [Required] public string CodigoCliente { get; set; } = string.Empty;
        [Required] public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        [Required] public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
        [Required] public bool Estado { get; set; }
    }
}

