namespace SuperEsperanzaApi.Services
{
    public static class RolPermissionsService
    {
        private static readonly Dictionary<string, List<string>> PermisosPorRol = new()
        {
            {
                "Administrador",
                new List<string>
                {
                    "Acceso completo a todos los módulos",
                    "Crear, leer, actualizar y eliminar en todos los endpoints",
                    "Gestionar usuarios y roles",
                    "Anular facturas",
                    "Cerrar sesiones de cualquier usuario"
                }
            },
            {
                "Gerente",
                new List<string>
                {
                    "Solo lectura (GET) en todos los módulos",
                    "Ver reportes y estadísticas",
                    "Consultar información de productos, clientes, facturas, compras",
                    "Ver sesiones activas",
                    "No puede crear, actualizar ni eliminar registros"
                }
            },
            {
                "Bodeguero",
                new List<string>
                {
                    "Leer: Productos, Categorías, Lotes, Compras, Proveedores",
                    "Crear y actualizar: Productos, Categorías, Lotes, Compras, Proveedores",
                    "Gestionar inventario",
                    "Registrar compras y lotes",
                    "No puede eliminar registros (solo Administrador puede eliminar)"
                }
            },
            {
                "Cajero",
                new List<string>
                {
                    "Leer: Clientes, Productos, Lotes, Facturas, Sesiones",
                    "Crear y actualizar: Clientes, Facturas, Sesiones",
                    "Abrir y cerrar su propia sesión de caja",
                    "Registrar ventas (facturas)",
                    "Gestionar clientes",
                    "No puede eliminar registros (solo Administrador puede eliminar)"
                }
            },
            {
                "Supervisor",
                new List<string>
                {
                    "Leer: Todos los módulos",
                    "Actualizar: Facturas (anular), Sesiones, Productos, Clientes",
                    "Corregir registros y anular facturas",
                    "Ajustar precios y stock de productos",
                    "Cerrar sesiones de cualquier usuario",
                    "No puede eliminar registros (solo Administrador puede eliminar)"
                }
            },
            {
                "Contador",
                new List<string>
                {
                    "Solo lectura de tablas financieras",
                    "Ver: Facturas, Compras, Sesiones, Productos, Clientes, Proveedores, Lotes",
                    "Consultar información financiera y de inventario",
                    "No puede acceder a información de Usuarios y Roles",
                    "No puede crear, actualizar ni eliminar registros"
                }
            }
        };

        public static List<string> ObtenerPermisosPorRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol))
                return new List<string> { "Rol no identificado" };

            return PermisosPorRol.TryGetValue(rol, out var permisos) 
                ? permisos 
                : new List<string> { $"Rol '{rol}' no tiene permisos definidos" };
        }

        public static List<string> ObtenerTodosLosRoles()
        {
            return PermisosPorRol.Keys.ToList();
        }
    }
}

