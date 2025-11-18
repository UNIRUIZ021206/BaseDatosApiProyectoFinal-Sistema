using System.Net.Http.Headers;

namespace SuperEsperanzaFrontEnd.Services
{
    public class HttpClientService
    {
        private static HttpClient? _httpClient;
        private static readonly object _lock = new object();
        private const string BaseUrl = "https://localhost:7022";

        public static HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                lock (_lock)
                {
                    if (_httpClient == null)
                    {
                        var handler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                        };
                        _httpClient = new HttpClient(handler)
                        {
                            BaseAddress = new Uri(BaseUrl),
                            Timeout = TimeSpan.FromSeconds(30)
                        };
                        _httpClient.DefaultRequestHeaders.Accept.Clear();
                        _httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                }
            }

            // Actualizar token en cada llamada
            var token = TokenService.ObtenerToken();
            
            // Siempre remover el header Authorization primero para evitar duplicados
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            
            if (!string.IsNullOrEmpty(token))
            {
                // Agregar el nuevo token
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return _httpClient;
        }
    }
}

