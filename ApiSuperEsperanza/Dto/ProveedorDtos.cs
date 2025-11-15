using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
{
    public class ProveedorDto
    {
        public int Id_Proveedor { get; set; }
        public string CodigoProveedor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
        public bool Estado { get; set; }
    }

    public class ProveedorCreateDto
    {
        [Required] public string CodigoProveedor { get; set; } = string.Empty;
        [Required] public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
    }

    public class ProveedorUpdateDto
    {
        [Required] public string CodigoProveedor { get; set; } = string.Empty;
        [Required] public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
        [Required] public bool Estado { get; set; }
    }
}

