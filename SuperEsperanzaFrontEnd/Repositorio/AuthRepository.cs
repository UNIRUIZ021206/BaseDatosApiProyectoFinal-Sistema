using System.Net.Http.Json;
using System.Text.Json;
using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio.Interfaces;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthRepository()
        {
            _httpClient = new HttpClient();
            _baseUrl = "https://localhost:7022"; // URL base de la API
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(jsonOptions);
                    
                    // Asegurar que la fecha de expiración esté en UTC
                    if (loginResponse != null && loginResponse.Expiracion.Kind != DateTimeKind.Utc)
                    {
                        loginResponse.Expiracion = DateTime.SpecifyKind(loginResponse.Expiracion, DateTimeKind.Utc);
                    }
                    
                    return loginResponse;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Intentar leer el mensaje de error
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<MensajeResponse>(errorContent);
                        throw new UnauthorizedAccessException(errorResponse?.Mensaje ?? "Credenciales inválidas");
                    }
                    catch
                    {
                        throw new UnauthorizedAccessException("Credenciales inválidas");
                    }
                }
                else
                {
                    // Intentar leer el mensaje de error
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                        throw new Exception(errorResponse?.Error ?? "Error al realizar el login");
                    }
                    catch
                    {
                        throw new Exception($"Error al realizar el login: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión con el servidor: {ex.Message}", ex);
            }
            catch (TaskCanceledException)
            {
                throw new Exception("La solicitud tardó demasiado. Verifique su conexión a internet.");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

