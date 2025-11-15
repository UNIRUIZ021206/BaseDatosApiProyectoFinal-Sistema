using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Services;
using System.Security.Claims;

namespace SuperEsperanzaApi.Filters
{
    public class AuthorizationErrorFilter : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationErrorFilter> _logger;

        public AuthorizationErrorFilter(ILogger<AuthorizationErrorFilter> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Este filtro se ejecuta después de la autorización
            // Si llegamos aquí y hay un problema de autorización, ya fue manejado
        }
    }

    public class AuthorizationErrorHandler : IAsyncAuthorizationFilter
    {
        private readonly ILogger<AuthorizationErrorHandler> _logger;

        public AuthorizationErrorHandler(ILogger<AuthorizationErrorHandler> logger)
        {
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Verificar si hay un problema de autorización
            if (context.Result is ForbidResult || context.Result is ChallengeResult)
            {
                var rol = context.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value ?? "No identificado";
                var endpoint = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                var accion = ObtenerAccionDesdeMetodo(context.HttpContext.Request.Method);

                var permisos = RolPermissionsService.ObtenerPermisosPorRol(rol);

                var errorResponse = new AuthorizationErrorResponse
                {
                    Mensaje = $"No tiene autorización para realizar esta acción. Su rol actual es: {rol}",
                    Rol = rol,
                    PermisosPermitidos = permisos,
                    AccionSolicitada = accion,
                    Endpoint = endpoint
                };

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 403
                };

                _logger.LogWarning(
                    "Acceso denegado para rol {Rol} en {Endpoint}. Acción: {Accion}",
                    rol, endpoint, accion);
            }
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

