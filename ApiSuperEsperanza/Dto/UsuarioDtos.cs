using System.ComponentModel.DataAnnotations;

namespace SuperEsperanzaApi.Dto
{
    public class UsuarioDto
    {
        public int Id_Usuario { get; set; }
        public string CodigoUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public int Id_Rol { get; set; }
        public string? NombreRol { get; set; }
        public bool Estado { get; set; }
    }

    public class UsuarioCreateDto
    {
        [Required] public string CodigoUsuario { get; set; } = string.Empty;
        [Required] public string Nombre { get; set; } = string.Empty;
        [Required] public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        [Required] public string Clave { get; set; } = string.Empty;
        [Required] public int Id_Rol { get; set; }
    }

    public class UsuarioUpdateDto
    {
        [Required] public string CodigoUsuario { get; set; } = string.Empty;
        [Required] public string Nombre { get; set; } = string.Empty;
        [Required] public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Clave { get; set; } // Opcional, si es null no se actualiza
        [Required] public int Id_Rol { get; set; }
        [Required] public bool Estado { get; set; }
    }
}

