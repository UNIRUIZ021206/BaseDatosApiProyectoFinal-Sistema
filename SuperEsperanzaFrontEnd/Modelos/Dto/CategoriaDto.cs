namespace SuperEsperanzaFrontEnd.Modelos.Dto
{
    public class CategoriaDto
    {
        public int Id_Categoria { get; set; }
        public string CodigoCategoria { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class CategoriaCreateDto
    {
        public string CodigoCategoria { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class CategoriaUpdateDto
    {
        public string CodigoCategoria { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}

