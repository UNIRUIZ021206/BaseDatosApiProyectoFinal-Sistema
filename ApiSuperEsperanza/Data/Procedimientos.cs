namespace SuperEsperanzaApi.Data
{
    internal static class Procedimientos
    {
        public const string SP_VALIDAR_USUARIO = "sp_ValidarUsuario_Super";

        // Roles
        public const string SP_LISTAR_ROLES = "sp_ListarRoles";
        public const string SP_OBTENER_ROL_POR_ID = "sp_ObtenerRolPorId";
        public const string SP_INSERTAR_ROL = "sp_InsertarRol";
        public const string SP_ACTUALIZAR_ROL = "sp_ActualizarRol";
        public const string SP_ELIMINAR_ROL = "sp_EliminarRol";

        // Categorías
        public const string SP_LISTAR_CATEGORIAS = "sp_ListarCategorias";
        public const string SP_OBTENER_CATEGORIA_POR_ID = "sp_ObtenerCategoriaPorId";
        public const string SP_INSERTAR_CATEGORIA = "sp_InsertarCategoria";
        public const string SP_ACTUALIZAR_CATEGORIA = "sp_ActualizarCategoria";
        public const string SP_ELIMINAR_CATEGORIA = "sp_EliminarCategoria";

        // Productos
        public const string SP_LISTAR_PRODUCTOS = "sp_ListarProductos";
        public const string SP_OBTENER_PRODUCTO_POR_ID = "sp_ObtenerProductoPorId";
        public const string SP_INSERTAR_PRODUCTO = "sp_InsertarProducto";
        public const string SP_ACTUALIZAR_PRODUCTO = "sp_ActualizarProducto";
        public const string SP_ELIMINAR_PRODUCTO = "sp_EliminarProducto";

        // Proveedores
        public const string SP_LISTAR_PROVEEDORES = "sp_ListarProveedores";
        public const string SP_OBTENER_PROVEEDOR_POR_ID = "sp_ObtenerProveedorPorId";
        public const string SP_INSERTAR_PROVEEDOR = "sp_InsertarProveedor";
        public const string SP_ACTUALIZAR_PROVEEDOR = "sp_ActualizarProveedor";
        public const string SP_ELIMINAR_PROVEEDOR = "sp_EliminarProveedor";

        // Clientes
        public const string SP_LISTAR_CLIENTES = "sp_ListarClientes";
        public const string SP_OBTENER_CLIENTE_POR_ID = "sp_ObtenerClientePorId";
        public const string SP_INSERTAR_CLIENTE = "sp_InsertarCliente";
        public const string SP_ACTUALIZAR_CLIENTE = "sp_ActualizarCliente";
        public const string SP_ELIMINAR_CLIENTE = "sp_EliminarCliente";

        // Usuarios (CRUD)
        public const string SP_LISTAR_USUARIOS = "sp_ListarUsuarios";
        public const string SP_OBTENER_USUARIO_POR_ID = "sp_ObtenerUsuarioPorId";
        public const string SP_INSERTAR_USUARIO = "sp_InsertarUsuario";
        public const string SP_ACTUALIZAR_USUARIO = "sp_ActualizarUsuario";
        public const string SP_ELIMINAR_USUARIO = "sp_EliminarUsuario";

        // Compras
        public const string SP_INSERTAR_COMPRA = "sp_InsertarCompra";
        public const string SP_INSERTAR_DETALLE_COMPRA = "sp_InsertarDetalleCompra";
        public const string SP_LISTAR_DETALLES_POR_COMPRA = "sp_ListarDetallesPorCompra";
        public const string SP_ACTUALIZAR_DETALLE_COMPRA = "sp_ActualizarDetalleCompra";
        public const string SP_ELIMINAR_DETALLE_COMPRA = "sp_EliminarDetalleCompra";

        // Lotes
        public const string SP_INSERTAR_LOTE = "sp_InsertarLote";
        public const string SP_LISTAR_LOTES_POR_PRODUCTO = "sp_ListarLotesPorProducto";
        public const string SP_OBTENER_LOTE_POR_ID = "sp_ObtenerLotePorId";

        // Sesiones
        public const string SP_LISTAR_SESIONES_ACTIVAS = "sp_ListarSesionesActivas";
        public const string SP_ABRIR_SESION = "sp_AbrirSesion";
        public const string SP_CERRAR_SESION = "sp_CerrarSesion";

        // Facturas
        public const string SP_INSERTAR_FACTURA = "sp_InsertarFactura";
        public const string SP_INSERTAR_DETALLE_FACTURA = "sp_InsertarDetalleFactura";
        public const string SP_LISTAR_DETALLES_POR_FACTURA = "sp_ListarDetallesPorFactura";
        public const string SP_ANULAR_FACTURA = "sp_AnularFactura";
        
        // Clientes - Puntos
        public const string SP_ASIGNAR_PUNTOS_CLIENTE = "sp_AsignarPuntosCliente";
        
        // Reportes
        public const string SP_REPORTE_VENTAS = "sp_ReporteVentas";
        public const string SP_TOP5_PRODUCTOS_MAS_VENDIDOS = "sp_Top5ProductosMasVendidos";
        public const string SP_TOP5_CATEGORIAS_MAS_VENDIDAS = "sp_Top5CategoriasMasVendidas";
        public const string SP_TOP5_PRODUCTOS_MAS_INGRESOS = "sp_Top5ProductosMasIngresos";
        
        // Lotes
        public const string SP_MARCAR_LOTES_VENCIDOS = "sp_MarcarLotesVencidos";
        
        // Vistas
        public const string VW_INVENTARIO_GENERAL = "vw_InventarioGeneral";
    }
}
