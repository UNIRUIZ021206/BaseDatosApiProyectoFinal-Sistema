namespace SuperEsperanzaApi.Dto
{
    public class AuthorizationErrorResponse
    {
        public string Mensaje { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public List<string> PermisosPermitidos { get; set; } = new();
        public string AccionSolicitada { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}

