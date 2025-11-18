namespace SuperEsperanzaFrontEnd.Modelos.Dto
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
        
        public string NombreCompleto => $"{P_Nombre} {S_Nombre ?? ""} {P_Apellido} {S_Apellido ?? ""}".Trim();
    }

    public class ClienteCreateDto
    {
        public string CodigoCliente { get; set; } = string.Empty;
        public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
    }

    public class ClienteUpdateDto
    {
        public string CodigoCliente { get; set; } = string.Empty;
        public string P_Nombre { get; set; } = string.Empty;
        public string? S_Nombre { get; set; }
        public string P_Apellido { get; set; } = string.Empty;
        public string? S_Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? TipoMembresia { get; set; }
        public bool Estado { get; set; }
    }
}

