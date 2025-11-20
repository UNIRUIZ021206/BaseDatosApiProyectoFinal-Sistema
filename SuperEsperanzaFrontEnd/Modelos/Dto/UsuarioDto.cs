namespace SuperEsperanzaFrontEnd.Modelos.Dto
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
        public string CodigoUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string Clave { get; set; } = string.Empty;
        public int Id_Rol { get; set; }
    }

    public class UsuarioUpdateDto
    {
        public string CodigoUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Clave { get; set; }
        public int Id_Rol { get; set; }
        public bool Estado { get; set; }
    }
}

