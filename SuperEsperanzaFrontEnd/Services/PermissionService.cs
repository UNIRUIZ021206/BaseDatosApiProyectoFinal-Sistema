namespace SuperEsperanzaFrontEnd.Services
{
    public static class PermissionService
    {
        /// <summary>
        /// Verifica si el usuario tiene acceso a un módulo específico
        /// </summary>
        public static bool TieneAcceso(string modulo)
        {
            var rol = TokenService.ObtenerRol();
            if (string.IsNullOrEmpty(rol))
                return false;

            return modulo switch
            {
                "Productos" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Cajero", "Gerente", "Contador"),
                "Categorias" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Gerente", "Contador"),
                "Clientes" => TieneRol(rol, "Administrador", "Cajero", "Supervisor", "Gerente", "Contador"),
                "Proveedores" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Gerente", "Contador"),
                "Compras" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Gerente", "Contador"),
                "Facturas" => TieneRol(rol, "Administrador", "Cajero", "Supervisor", "Gerente", "Contador"),
                "Lotes" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Gerente", "Contador"),
                "Sesiones" => TieneRol(rol, "Administrador", "Cajero", "Supervisor", "Gerente", "Contador"),
                "Usuarios" => TieneRol(rol, "Administrador"),
                "Roles" => TieneRol(rol, "Administrador"),
                _ => false
            };
        }

        /// <summary>
        /// Verifica si el usuario puede crear en un módulo
        /// </summary>
        public static bool PuedeCrear(string modulo)
        {
            var rol = TokenService.ObtenerRol();
            if (string.IsNullOrEmpty(rol))
                return false;

            return modulo switch
            {
                "Productos" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Categorias" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Clientes" => TieneRol(rol, "Administrador", "Cajero", "Supervisor"),
                "Proveedores" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Compras" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Facturas" => TieneRol(rol, "Administrador", "Cajero"),
                "Lotes" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Sesiones" => TieneRol(rol, "Administrador", "Cajero"),
                "Usuarios" => TieneRol(rol, "Administrador"),
                "Roles" => TieneRol(rol, "Administrador"),
                _ => false
            };
        }

        /// <summary>
        /// Verifica si el usuario puede actualizar en un módulo
        /// </summary>
        public static bool PuedeActualizar(string modulo)
        {
            var rol = TokenService.ObtenerRol();
            if (string.IsNullOrEmpty(rol))
                return false;

            return modulo switch
            {
                "Productos" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor"),
                "Categorias" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor"),
                "Clientes" => TieneRol(rol, "Administrador", "Cajero", "Supervisor"),
                "Proveedores" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor"),
                "Compras" => TieneRol(rol, "Administrador", "Bodeguero"),
                "Facturas" => TieneRol(rol, "Administrador", "Supervisor"), // Anular facturas
                "Lotes" => TieneRol(rol, "Administrador", "Bodeguero", "Supervisor"),
                "Sesiones" => TieneRol(rol, "Administrador", "Cajero", "Supervisor"), // Cerrar sesiones
                "Usuarios" => TieneRol(rol, "Administrador"),
                "Roles" => TieneRol(rol, "Administrador"),
                _ => false
            };
        }

        /// <summary>
        /// Verifica si el usuario puede eliminar en un módulo
        /// </summary>
        public static bool PuedeEliminar(string modulo)
        {
            var rol = TokenService.ObtenerRol();
            if (string.IsNullOrEmpty(rol))
                return false;

            // Solo Administrador puede eliminar
            return TieneRol(rol, "Administrador");
        }

        /// <summary>
        /// Verifica si el usuario puede usar el Punto de Venta
        /// </summary>
        public static bool PuedeUsarPuntoVenta()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Cajero");
        }

        /// <summary>
        /// Verifica si el usuario puede abrir sesiones de caja
        /// </summary>
        public static bool PuedeAbrirSesion()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Cajero");
        }

        /// <summary>
        /// Verifica si el usuario puede cerrar sesiones de caja
        /// </summary>
        public static bool PuedeCerrarSesion()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Cajero", "Supervisor");
        }

        /// <summary>
        /// Verifica si el usuario puede anular facturas
        /// </summary>
        public static bool PuedeAnularFactura()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Supervisor");
        }

        /// <summary>
        /// Verifica si el usuario puede ver reportes de ventas
        /// </summary>
        public static bool PuedeVerReporteVentas()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Supervisor", "Gerente", "Contador");
        }

        /// <summary>
        /// Verifica si el usuario puede ver reportes de inventario
        /// </summary>
        public static bool PuedeVerReporteInventario()
        {
            var rol = TokenService.ObtenerRol();
            return TieneRol(rol, "Administrador", "Bodeguero", "Supervisor", "Gerente", "Contador");
        }

        /// <summary>
        /// Verifica si el rol del usuario está en la lista de roles permitidos
        /// </summary>
        private static bool TieneRol(string? rol, params string[] rolesPermitidos)
        {
            if (string.IsNullOrEmpty(rol))
                return false;

            return rolesPermitidos.Any(r => r.Equals(rol, StringComparison.OrdinalIgnoreCase));
        }
    }
}

