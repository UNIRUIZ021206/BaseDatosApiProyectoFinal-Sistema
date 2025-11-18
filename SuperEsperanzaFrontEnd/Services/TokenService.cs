namespace SuperEsperanzaFrontEnd.Services
{
    public class TokenService
    {
        private static string? _token;
        private static string? _usuario;
        private static string? _rol;
        private static DateTime _expiracion;

        public static void GuardarToken(string token, string usuario, string rol, DateTime expiracion)
        {
            _token = token;
            _usuario = usuario;
            _rol = rol;
            _expiracion = expiracion;
        }

        public static string? ObtenerToken()
        {
            if (_token == null)
            {
                return null;
            }

            // Verificar que el token no esté expirado
            // Usamos un margen de 10 segundos para evitar problemas de sincronización
            var tiempoRestante = _expiracion - DateTime.UtcNow;
            if (tiempoRestante.TotalSeconds > 10)
            {
                return _token;
            }
            
            // Token expirado o próximo a expirar (menos de 10 segundos)
            return null;
        }

        public static string? ObtenerUsuario()
        {
            return _usuario;
        }

        public static string? ObtenerRol()
        {
            return _rol;
        }

        public static bool EstaAutenticado()
        {
            if (_token == null)
            {
                return false;
            }

            // Verificar que el token no esté expirado (margen de 10 segundos)
            var tiempoRestante = _expiracion - DateTime.UtcNow;
            return tiempoRestante.TotalSeconds > 10;
        }

        public static void CerrarSesion()
        {
            _token = null;
            _usuario = null;
            _rol = null;
            _expiracion = DateTime.MinValue;
        }

        // Método de depuración para verificar el estado del token
        public static string? ObtenerInfoToken()
        {
            if (_token == null)
            {
                return "Token: null";
            }

            var tiempoRestante = _expiracion - DateTime.UtcNow;
            return $"Token existe. Expira en: {_expiracion:yyyy-MM-dd HH:mm:ss} UTC. Tiempo restante: {tiempoRestante.TotalMinutes:F2} minutos";
        }
    }
}

