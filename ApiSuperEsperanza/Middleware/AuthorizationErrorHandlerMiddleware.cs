using Microsoft.AspNetCore.Authorization;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SuperEsperanzaApi.Middleware
{
    public class AuthorizationErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationErrorHandlerMiddleware> _logger;

        public AuthorizationErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<AuthorizationErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Guardar el stream original de respuesta
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Si la respuesta es 403 (Forbidden), personalizamos el mensaje
            if (context.Response.StatusCode == 403)
            {
                var rol = context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "No identificado";
                var endpoint = $"{context.Request.Method} {context.Request.Path}";
                var accion = ObtenerAccionDesdeMetodo(context.Request.Method);

                var permisos = RolPermissionsService.ObtenerPermisosPorRol(rol);

                var errorResponse = new AuthorizationErrorResponse
                {
                    Mensaje = $"No tiene autorización para realizar esta acción. Su rol actual es: {rol}",
                    Rol = rol,
                    PermisosPermitidos = permisos,
                    AccionSolicitada = accion,
                    Endpoint = endpoint
                };

                // Limpiar el cuerpo de respuesta anterior
                responseBody.SetLength(0);
                context.Response.ContentType = "application/json";
                
                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await context.Response.WriteAsync(json);
                
                _logger.LogWarning(
                    "Acceso denegado para rol {Rol} en {Endpoint}. Acción: {Accion}",
                    rol, endpoint, accion);
            }

            // Copiar el contenido del nuevo stream al original
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private string ObtenerAccionDesdeMetodo(string metodo)
        {
            return metodo switch
            {
                "GET" => "Consultar/Leer",
                "POST" => "Crear",
                "PUT" => "Actualizar",
                "PATCH" => "Actualizar parcialmente",
                "DELETE" => "Eliminar",
                _ => metodo
            };
        }
    }
}

