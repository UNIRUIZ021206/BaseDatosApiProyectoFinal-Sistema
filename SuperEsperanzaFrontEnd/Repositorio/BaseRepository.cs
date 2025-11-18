using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SuperEsperanzaFrontEnd.Services;

namespace SuperEsperanzaFrontEnd.Repositorio
{
    public abstract class BaseRepository
    {
        protected readonly string _baseUrl;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected BaseRepository()
        {
            _baseUrl = "https://localhost:7022";
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                // Configurar formato de fecha para evitar problemas con SQL Server
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                // Configurar el formato de fecha para que se serialice correctamente
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };
        }

        protected HttpClient GetHttpClient()
        {
            // Obtener el cliente HTTP compartido (esto actualiza el token automáticamente)
            return Services.HttpClientService.GetClient();
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                // Verificar que el token esté presente antes de hacer la petición
                var token = TokenService.ObtenerToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No hay token de autenticación. Por favor, inicie sesión nuevamente.");
                }

                var httpClient = GetHttpClient();
                var response = await httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Limpiar el token si recibimos un 401
                    TokenService.CerrarSesion();
                    throw new UnauthorizedAccessException("Sesión expirada. Por favor, inicie sesión nuevamente.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
        }

        protected async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            try
            {
                // Verificar que el token esté presente antes de hacer la petición
                var token = TokenService.ObtenerToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No hay token de autenticación. Por favor, inicie sesión nuevamente.");
                }

                var httpClient = GetHttpClient();
                var response = await httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<T>>(_jsonOptions);
                    return result ?? new List<T>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Limpiar el token si recibimos un 401
                    TokenService.CerrarSesion();
                    throw new UnauthorizedAccessException("Sesión expirada. Por favor, inicie sesión nuevamente.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
        }

        protected async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                // Verificar que el token esté presente antes de hacer la petición
                var token = TokenService.ObtenerToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No hay token de autenticación. Por favor, inicie sesión nuevamente.");
                }

                var httpClient = GetHttpClient();
                var response = await httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Limpiar el token si recibimos un 401
                    TokenService.CerrarSesion();
                    throw new UnauthorizedAccessException("Sesión expirada. Por favor, inicie sesión nuevamente.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<Modelos.Dto.ErrorResponse>(errorContent, _jsonOptions);
                        throw new Exception(errorResponse?.Error ?? "Error en la operación");
                    }
                    catch
                    {
                        throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
        }

        protected async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                // Verificar que el token esté presente antes de hacer la petición
                var token = TokenService.ObtenerToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No hay token de autenticación. Por favor, inicie sesión nuevamente.");
                }

                var httpClient = GetHttpClient();
                var response = await httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Limpiar el token si recibimos un 401
                    TokenService.CerrarSesion();
                    throw new UnauthorizedAccessException("Sesión expirada. Por favor, inicie sesión nuevamente.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<Modelos.Dto.ErrorResponse>(errorContent, _jsonOptions);
                        throw new Exception(errorResponse?.Error ?? "Error en la operación");
                    }
                    catch
                    {
                        throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
        }

        protected async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                // Verificar que el token esté presente antes de hacer la petición
                var token = TokenService.ObtenerToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No hay token de autenticación. Por favor, inicie sesión nuevamente.");
                }

                var httpClient = GetHttpClient();
                var response = await httpClient.DeleteAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Limpiar el token si recibimos un 401
                    TokenService.CerrarSesion();
                    throw new UnauthorizedAccessException("Sesión expirada. Por favor, inicie sesión nuevamente.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<Modelos.Dto.ErrorResponse>(errorContent, _jsonOptions);
                        throw new Exception(errorResponse?.Error ?? "Error en la operación");
                    }
                    catch
                    {
                        throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
        }
    }
}

