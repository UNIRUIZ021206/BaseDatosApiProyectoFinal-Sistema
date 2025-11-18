-- =============================================
-- QUERIES DE TODOS LOS STORED PROCEDURES USADOS POR LA API
-- =============================================
-- Este archivo contiene las definiciones SQL de todos los stored procedures
-- que utiliza la API SuperEsperanza, basado en los parámetros usados en el código C#
-- =============================================

-- =============================================
-- 1. AUTENTICACIÓN
-- =============================================

-- sp_ValidarUsuario_Super
-- Parámetros: @CodigoUsuario (NVARCHAR), @Contrasena (NVARCHAR)
-- Retorna: IdUsuario, NombreUsuario, NombreRol
EXEC sp_ValidarUsuario_Super @CodigoUsuario = 'USR001', @Contrasena = 'password123';

-- =============================================
-- 2. ROLES
-- =============================================

-- sp_ListarRoles
-- Sin parámetros
-- Retorna: Id_Rol, NombreRol
EXEC sp_ListarRoles;

-- sp_ObtenerRolPorId
-- Parámetros: @Id_Rol (INT)
-- Retorna: Id_Rol, NombreRol, FechaCreacion, FechaModificacion
EXEC sp_ObtenerRolPorId @Id_Rol = 1;

-- sp_InsertarRol
-- Parámetros: @NombreRol (VARCHAR(50)), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Rol (nuevo ID insertado)
EXEC sp_InsertarRol @NombreRol = 'Administrador', @Id_UsuarioCreacion = 1;

-- sp_ActualizarRol
-- Parámetros: @Id_Rol (INT), @NombreRol (VARCHAR(50)), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso, 0 si falla
EXEC sp_ActualizarRol @Id_Rol = 1, @NombreRol = 'Administrador', @Id_UsuarioModificacion = 1;

-- sp_EliminarRol
-- Parámetros: @Id_Rol (INT)
-- Retorna: 1 si exitoso, 0 si falla
EXEC sp_EliminarRol @Id_Rol = 1;

-- =============================================
-- 3. CATEGORÍAS
-- =============================================

-- sp_ListarCategorias
-- Sin parámetros
-- Retorna: Id_Categoria, CodigoCategoria, Nombre, Descripcion, Estado, FechaCreacion, Id_UsuarioCreacion
EXEC sp_ListarCategorias;

-- sp_ObtenerCategoriaPorId
-- Parámetros: @Id_Categoria (INT)
-- Retorna: Id_Categoria, CodigoCategoria, Nombre, Descripcion, Estado, FechaCreacion, Id_UsuarioCreacion
EXEC sp_ObtenerCategoriaPorId @Id_Categoria = 1;

-- sp_InsertarCategoria
-- Parámetros: @CodigoCategoria (VARCHAR), @Nombre (VARCHAR), @Descripcion (VARCHAR, NULL), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Categoria (nuevo ID insertado)
EXEC sp_InsertarCategoria 
    @CodigoCategoria = 'CAT001', 
    @Nombre = 'Categoría 1', 
    @Descripcion = 'Descripción', 
    @Id_UsuarioCreacion = 1;

-- sp_ActualizarCategoria
-- Parámetros: @Id_Categoria (INT), @CodigoCategoria (VARCHAR), @Nombre (VARCHAR), @Descripcion (VARCHAR, NULL), @Estado (BIT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_ActualizarCategoria 
    @Id_Categoria = 1, 
    @CodigoCategoria = 'CAT001', 
    @Nombre = 'Categoría Actualizada', 
    @Descripcion = 'Nueva descripción', 
    @Estado = 1, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarCategoria
-- Parámetros: @Id_Categoria (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_EliminarCategoria @Id_Categoria = 1, @Id_UsuarioModificacion = 1;

-- =============================================
-- 4. PRODUCTOS
-- =============================================

-- sp_ListarProductos
-- Sin parámetros
-- Retorna: Id_Producto, CodigoProducto, Id_Categoria, NombreCategoria, Nombre, Descripcion, PrecioVenta, StockActual, CostoPromedio, Estado, FechaCreacion, FechaModificacion, Id_UsuarioCreacion, Id_UsuarioModificacion
EXEC sp_ListarProductos;

-- sp_ObtenerProductoPorId
-- Parámetros: @Id_Producto (INT)
-- Retorna: Id_Producto, CodigoProducto, Id_Categoria, NombreCategoria, Nombre, Descripcion, PrecioVenta, StockActual, CostoPromedio, Estado, FechaCreacion, FechaModificacion, Id_UsuarioCreacion, Id_UsuarioModificacion
EXEC sp_ObtenerProductoPorId @Id_Producto = 1;

-- sp_InsertarProducto
-- Parámetros: @CodigoProducto (VARCHAR), @Id_Categoria (INT), @Nombre (VARCHAR), @Descripcion (VARCHAR, NULL), @PrecioVenta (DECIMAL), @StockActual (INT), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Producto (nuevo ID insertado)
EXEC sp_InsertarProducto 
    @CodigoProducto = 'PROD001', 
    @Id_Categoria = 1, 
    @Nombre = 'Producto 1', 
    @Descripcion = 'Descripción del producto', 
    @PrecioVenta = 100.00, 
    @StockActual = 50, 
    @Id_UsuarioCreacion = 1;

-- sp_ActualizarProducto
-- Parámetros: @Id_Producto (INT), @CodigoProducto (VARCHAR), @Id_Categoria (INT), @Nombre (VARCHAR), @Descripcion (VARCHAR, NULL), @PrecioVenta (DECIMAL), @StockActual (INT), @Estado (BIT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_ActualizarProducto 
    @Id_Producto = 1, 
    @CodigoProducto = 'PROD001', 
    @Id_Categoria = 1, 
    @Nombre = 'Producto Actualizado', 
    @Descripcion = 'Nueva descripción', 
    @PrecioVenta = 120.00, 
    @StockActual = 60, 
    @Estado = 1, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarProducto
-- Parámetros: @Id_Producto (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_EliminarProducto @Id_Producto = 1, @Id_UsuarioModificacion = 1;

-- =============================================
-- 5. PROVEEDORES
-- =============================================

-- sp_ListarProveedores
-- Sin parámetros
-- Retorna: Id_Proveedor, CodigoProveedor, Nombre, Telefono, Email, Contacto, Estado, FechaCreacion, FechaModificacion, Id_UsuarioCreacion, Id_UsuarioModificacion
EXEC sp_ListarProveedores;

-- sp_ObtenerProveedorPorId
-- Parámetros: @Id_Proveedor (INT)
-- Retorna: Id_Proveedor, CodigoProveedor, Nombre, Telefono, Email, Contacto, Estado, FechaCreacion, FechaModificacion, Id_UsuarioCreacion, Id_UsuarioModificacion
EXEC sp_ObtenerProveedorPorId @Id_Proveedor = 1;

-- sp_InsertarProveedor
-- Parámetros: @CodigoProveedor (VARCHAR), @Nombre (VARCHAR), @Telefono (VARCHAR, NULL), @Email (VARCHAR, NULL), @Contacto (VARCHAR, NULL), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Proveedor (nuevo ID insertado)
EXEC sp_InsertarProveedor 
    @CodigoProveedor = 'PROV001', 
    @Nombre = 'Proveedor 1', 
    @Telefono = '1234567890', 
    @Email = 'proveedor@example.com', 
    @Contacto = 'Juan Pérez', 
    @Id_UsuarioCreacion = 1;

-- sp_ActualizarProveedor
-- Parámetros: @Id_Proveedor (INT), @CodigoProveedor (VARCHAR), @Nombre (VARCHAR), @Telefono (VARCHAR, NULL), @Email (VARCHAR, NULL), @Contacto (VARCHAR, NULL), @Estado (BIT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_ActualizarProveedor 
    @Id_Proveedor = 1, 
    @CodigoProveedor = 'PROV001', 
    @Nombre = 'Proveedor Actualizado', 
    @Telefono = '0987654321', 
    @Email = 'nuevo@example.com', 
    @Contacto = 'María García', 
    @Estado = 1, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarProveedor
-- Parámetros: @Id_Proveedor (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_EliminarProveedor @Id_Proveedor = 1, @Id_UsuarioModificacion = 1;

-- =============================================
-- 6. CLIENTES
-- =============================================

-- sp_ListarClientes
-- Sin parámetros
-- Retorna: Id_Cliente, CodigoCliente, P_Nombre, S_Nombre, P_Apellido, S_Apellido, Telefono, Direccion, Email, TipoMembresia, Estado, FechaRegistro, PuntosCompra, UltimaCompra, Id_UsuarioCreacion, Id_UsuarioModificacion, FechaModificacion
EXEC sp_ListarClientes;

-- sp_ObtenerClientePorId
-- Parámetros: @Id_Cliente (INT)
-- Retorna: Id_Cliente, CodigoCliente, P_Nombre, S_Nombre, P_Apellido, S_Apellido, Telefono, Direccion, Email, TipoMembresia, Estado, FechaRegistro, PuntosCompra, UltimaCompra, Id_UsuarioCreacion, Id_UsuarioModificacion, FechaModificacion
EXEC sp_ObtenerClientePorId @Id_Cliente = 1;

-- sp_InsertarCliente
-- Parámetros: @CodigoCliente (VARCHAR), @P_Nombre (VARCHAR), @S_Nombre (VARCHAR, NULL), @P_Apellido (VARCHAR), @S_Apellido (VARCHAR, NULL), @Telefono (VARCHAR, NULL), @Direccion (VARCHAR, NULL), @Email (VARCHAR, NULL), @TipoMembresia (VARCHAR, NULL), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Cliente (nuevo ID insertado)
EXEC sp_InsertarCliente 
    @CodigoCliente = 'CLI001', 
    @P_Nombre = 'Juan', 
    @S_Nombre = 'Carlos', 
    @P_Apellido = 'Pérez', 
    @S_Apellido = 'García', 
    @Telefono = '1234567890', 
    @Direccion = 'Calle 123', 
    @Email = 'cliente@example.com', 
    @TipoMembresia = 'VIP', 
    @Id_UsuarioCreacion = 1;

-- sp_ActualizarCliente
-- Parámetros: @Id_Cliente (INT), @CodigoCliente (VARCHAR), @P_Nombre (VARCHAR), @S_Nombre (VARCHAR, NULL), @P_Apellido (VARCHAR), @S_Apellido (VARCHAR, NULL), @Telefono (VARCHAR, NULL), @Direccion (VARCHAR, NULL), @Email (VARCHAR, NULL), @TipoMembresia (VARCHAR, NULL), @Estado (BIT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_ActualizarCliente 
    @Id_Cliente = 1, 
    @CodigoCliente = 'CLI001', 
    @P_Nombre = 'Juan', 
    @S_Nombre = 'Carlos', 
    @P_Apellido = 'Pérez', 
    @S_Apellido = 'García', 
    @Telefono = '0987654321', 
    @Direccion = 'Nueva Calle 456', 
    @Email = 'nuevo@example.com', 
    @TipoMembresia = 'Premium', 
    @Estado = 1, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarCliente
-- Parámetros: @Id_Cliente (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_EliminarCliente @Id_Cliente = 1, @Id_UsuarioModificacion = 1;

-- sp_AsignarPuntosCliente
-- Parámetros: @Id_Cliente (INT), @MontoTotalCompra (DECIMAL), @Id_UsuarioOperacion (INT)
-- Retorna: PuntosGanadosEnEstaCompra, Membresia
EXEC sp_AsignarPuntosCliente 
    @Id_Cliente = 1, 
    @MontoTotalCompra = 500.00, 
    @Id_UsuarioOperacion = 1;

-- =============================================
-- 7. USUARIOS (CRUD)
-- =============================================

-- sp_ListarUsuarios
-- Sin parámetros
-- Retorna: Id_Usuario, CodigoUsuario, Nombre, Apellido, Email, Estado, NombreRol
EXEC sp_ListarUsuarios;

-- sp_ObtenerUsuarioPorId
-- Parámetros: @Id_Usuario (INT)
-- Retorna: Id_Usuario, CodigoUsuario, Nombre, Apellido, Email, Telefono, Id_Rol, Estado
EXEC sp_ObtenerUsuarioPorId @Id_Usuario = 1;

-- sp_InsertarUsuario
-- Parámetros: @CodigoUsuario (VARCHAR(10)), @Nombre (VARCHAR(50)), @Apellido (VARCHAR(50)), @Email (VARCHAR(100), NULL), @Telefono (VARCHAR(15), NULL), @Clave (VARCHAR(100)), @Id_Rol (INT), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Usuario (nuevo ID insertado)
EXEC sp_InsertarUsuario 
    @CodigoUsuario = 'USR001', 
    @Nombre = 'Juan', 
    @Apellido = 'Pérez', 
    @Email = 'juan@example.com', 
    @Telefono = '1234567890', 
    @Clave = 'password123', 
    @Id_Rol = 1, 
    @Id_UsuarioCreacion = 1;

-- sp_ActualizarUsuario
-- Parámetros: @Id_Usuario (INT), @CodigoUsuario (VARCHAR(10)), @Nombre (VARCHAR(50)), @Apellido (VARCHAR(50)), @Email (VARCHAR(100), NULL), @Telefono (VARCHAR(15), NULL), @Clave (VARCHAR(100), NULL), @Id_Rol (INT), @Estado (BIT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso, 0 si falla
EXEC sp_ActualizarUsuario 
    @Id_Usuario = 1, 
    @CodigoUsuario = 'USR001', 
    @Nombre = 'Juan', 
    @Apellido = 'Pérez García', 
    @Email = 'juan.nuevo@example.com', 
    @Telefono = '0987654321', 
    @Clave = NULL, -- NULL si no se actualiza la contraseña
    @Id_Rol = 2, 
    @Estado = 1, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarUsuario
-- Parámetros: @Id_Usuario (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso, 0 si falla
EXEC sp_EliminarUsuario @Id_Usuario = 1, @Id_UsuarioModificacion = 1;

-- =============================================
-- 8. COMPRAS
-- =============================================

-- sp_InsertarCompra
-- Parámetros: @CodigoCompra (VARCHAR), @Id_Proveedor (INT), @FechaCompra (DATETIME), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Compra (nuevo ID insertado)
EXEC sp_InsertarCompra 
    @CodigoCompra = 'COMP001', 
    @Id_Proveedor = 1, 
    @FechaCompra = '2024-01-15', 
    @Id_UsuarioCreacion = 1;

-- sp_InsertarDetalleCompra
-- Parámetros: @Id_Compra (INT), @Id_Producto (INT), @Cantidad (INT), @PrecioUnitario (DECIMAL), @Id_UsuarioCreacion (INT)
-- Sin retorno (ExecuteNonQuery)
EXEC sp_InsertarDetalleCompra 
    @Id_Compra = 1, 
    @Id_Producto = 1, 
    @Cantidad = 10, 
    @PrecioUnitario = 50.00, 
    @Id_UsuarioCreacion = 1;

-- sp_ListarDetallesPorCompra
-- Parámetros: @Id_Compra (INT)
-- Retorna: Id_DetalleCompra, Id_Compra, Id_Producto, NombreProducto, Cantidad, PrecioUnitario, Subtotal
EXEC sp_ListarDetallesPorCompra @Id_Compra = 1;

-- sp_ActualizarDetalleCompra
-- Parámetros: @Id_DetalleCompra (INT), @Cantidad (INT), @PrecioUnitario (DECIMAL), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso
EXEC sp_ActualizarDetalleCompra 
    @Id_DetalleCompra = 1, 
    @Cantidad = 15, 
    @PrecioUnitario = 55.00, 
    @Id_UsuarioModificacion = 1;

-- sp_EliminarDetalleCompra
-- Parámetros: @Id_DetalleCompra (INT)
-- Retorna: 1 si exitoso
EXEC sp_EliminarDetalleCompra @Id_DetalleCompra = 1;

-- =============================================
-- 9. LOTES
-- =============================================

-- sp_InsertarLote
-- Parámetros: @CodigoLote (VARCHAR), @Id_Producto (INT), @Id_Compra (INT), @Cantidad (INT), @FechaVencimiento (DATETIME, NULL), @Id_UsuarioCreacion (INT)
-- Nota: @FechaIngreso no se pasa porque el SP usa GETDATE() automáticamente
-- Retorna: Id_Lote (nuevo ID insertado)
EXEC sp_InsertarLote 
    @CodigoLote = 'LOT001', 
    @Id_Producto = 1, 
    @Id_Compra = 1, 
    @Cantidad = 100, 
    @FechaVencimiento = '2024-12-31', 
    @Id_UsuarioCreacion = 1;

-- sp_ListarLotesPorProducto
-- Parámetros: @Id_Producto (INT)
-- Retorna: Id_Lote, CodigoLote, Id_Producto, Cantidad, FechaIngreso, FechaVencimiento, Estado
EXEC sp_ListarLotesPorProducto @Id_Producto = 1;

-- sp_ObtenerLotePorId
-- Parámetros: @Id_Lote (INT)
-- Retorna: Id_Lote, CodigoLote, Id_Producto, Id_Compra, Cantidad, FechaIngreso, FechaVencimiento, Estado, FechaCreacion, Id_UsuarioCreacion
EXEC sp_ObtenerLotePorId @Id_Lote = 1;

-- sp_MarcarLotesVencidos
-- Sin parámetros
-- Sin retorno (ExecuteNonQuery) - Marca los lotes vencidos como inactivos
EXEC sp_MarcarLotesVencidos;

-- =============================================
-- 10. SESIONES
-- =============================================

-- sp_ListarSesionesActivas
-- Sin parámetros
-- Retorna: Id_Sesion, CodigoSesion, UsuarioNombre, FechaApertura, MontoInicial, Estado
EXEC sp_ListarSesionesActivas;

-- sp_AbrirSesion
-- Parámetros: @CodigoSesion (VARCHAR), @Id_Usuario (INT), @MontoInicial (DECIMAL), @Observacion (VARCHAR, NULL)
-- Retorna: Id_Sesion (nuevo ID insertado)
EXEC sp_AbrirSesion 
    @CodigoSesion = 'SES001', 
    @Id_Usuario = 1, 
    @MontoInicial = 1000.00, 
    @Observacion = 'Sesión de apertura';

-- sp_CerrarSesion
-- Parámetros: @Id_Sesion (INT), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso, 0 si falla
EXEC sp_CerrarSesion @Id_Sesion = 1, @Id_UsuarioModificacion = 1;

-- =============================================
-- 11. FACTURAS
-- =============================================

-- sp_InsertarFactura
-- Parámetros: @CodigoFactura (VARCHAR), @Id_Cliente (INT, NULL), @Id_Sesion (INT), @NumeroFactura (VARCHAR), @Subtotal (DECIMAL), @Descuento (DECIMAL), @Impuesto (DECIMAL), @Id_UsuarioCreacion (INT)
-- Retorna: Id_Factura (nuevo ID insertado)
EXEC sp_InsertarFactura 
    @CodigoFactura = 'FAC001', 
    @Id_Cliente = 1, 
    @Id_Sesion = 1, 
    @NumeroFactura = '001-001-0000001', 
    @Subtotal = 500.00, 
    @Descuento = 50.00, 
    @Impuesto = 81.00, 
    @Id_UsuarioCreacion = 1;

-- sp_InsertarDetalleFactura
-- Parámetros: @Id_Factura (INT), @Id_Lote (INT), @Cantidad (INT), @PrecioUnitario (DECIMAL), @Id_UsuarioCreacion (INT)
-- Retorna: 1 si exitoso, -1 si hay error
-- Nota: Este SP valida stock, actualiza el stock del lote y maneja locks para evitar condiciones de carrera
-- Ver archivo: sp_InsertarDetalleFactura_Mejorado.sql para la definición completa
EXEC sp_InsertarDetalleFactura 
    @Id_Factura = 1, 
    @Id_Lote = 1, 
    @Cantidad = 5, 
    @PrecioUnitario = 100.00, 
    @Id_UsuarioCreacion = 1;

-- sp_ListarDetallesPorFactura
-- Parámetros: @Id_Factura (INT)
-- Retorna: Id_Detalle, Id_Factura, Id_Lote, CodigoLote, NombreProducto, Cantidad, PrecioUnitario, Subtotal
EXEC sp_ListarDetallesPorFactura @Id_Factura = 1;

-- sp_AnularFactura
-- Parámetros: @Id_Factura (INT), @Motivo (VARCHAR), @Id_UsuarioModificacion (INT)
-- Retorna: 1 si exitoso, -1 si hay error (factura no existe o ya está anulada)
EXEC sp_AnularFactura 
    @Id_Factura = 1, 
    @Motivo = 'Error en facturación', 
    @Id_UsuarioModificacion = 1;

-- =============================================
-- 12. REPORTES
-- =============================================

-- sp_ReporteVentas
-- Parámetros: @Fecha (DATE)
-- Retorna: Producto, CantidadVendida, TotalVenta
EXEC sp_ReporteVentas @Fecha = '2024-01-15';

-- sp_Top5ProductosMasVendidos
-- Parámetros: @FechaInicio (DATE, NULL), @FechaFin (DATE, NULL)
-- Retorna: Id_Producto, CodigoProducto, Producto, Categoria, CantidadVendida, TotalIngresos, NumeroFacturas
-- Nota: Si no se proporcionan fechas, retorna todos los registros históricos
EXEC sp_Top5ProductosMasVendidos;
-- O con rango de fechas:
EXEC sp_Top5ProductosMasVendidos @FechaInicio = '2024-01-01', @FechaFin = '2024-12-31';

-- sp_Top5CategoriasMasVendidas
-- Parámetros: @FechaInicio (DATE, NULL), @FechaFin (DATE, NULL)
-- Retorna: Id_Categoria, CodigoCategoria, Categoria, CantidadVendida, TotalIngresos, NumeroProductos, NumeroFacturas
-- Nota: Si no se proporcionan fechas, retorna todos los registros históricos
EXEC sp_Top5CategoriasMasVendidas;
-- O con rango de fechas:
EXEC sp_Top5CategoriasMasVendidas @FechaInicio = '2024-01-01', @FechaFin = '2024-12-31';

-- sp_Top5ProductosMasIngresos
-- Parámetros: @FechaInicio (DATE, NULL), @FechaFin (DATE, NULL)
-- Retorna: Id_Producto, CodigoProducto, Producto, Categoria, CantidadVendida, TotalIngresos, PrecioPromedio, NumeroFacturas
-- Nota: Si no se proporcionan fechas, retorna todos los registros históricos
EXEC sp_Top5ProductosMasIngresos;
-- O con rango de fechas:
EXEC sp_Top5ProductosMasIngresos @FechaInicio = '2024-01-01', @FechaFin = '2024-12-31';

-- =============================================
-- 13. VISTAS
-- =============================================

-- vw_InventarioGeneral
-- Sin parámetros (es una vista, no un SP)
-- Retorna: Id_Producto, CodigoProducto, NombreProducto, NombreCategoria, PrecioVenta, StockActual, StockDisponibleEnLotes, EstadoProducto
SELECT * FROM vw_InventarioGeneral;

-- =============================================
-- RESUMEN DE STORED PROCEDURES
-- =============================================
/*
TOTAL DE STORED PROCEDURES: 40

1. Autenticación: 1
   - sp_ValidarUsuario_Super

2. Roles: 5
   - sp_ListarRoles
   - sp_ObtenerRolPorId
   - sp_InsertarRol
   - sp_ActualizarRol
   - sp_EliminarRol

3. Categorías: 5
   - sp_ListarCategorias
   - sp_ObtenerCategoriaPorId
   - sp_InsertarCategoria
   - sp_ActualizarCategoria
   - sp_EliminarCategoria

4. Productos: 5
   - sp_ListarProductos
   - sp_ObtenerProductoPorId
   - sp_InsertarProducto
   - sp_ActualizarProducto
   - sp_EliminarProducto

5. Proveedores: 5
   - sp_ListarProveedores
   - sp_ObtenerProveedorPorId
   - sp_InsertarProveedor
   - sp_ActualizarProveedor
   - sp_EliminarProveedor

6. Clientes: 6
   - sp_ListarClientes
   - sp_ObtenerClientePorId
   - sp_InsertarCliente
   - sp_ActualizarCliente
   - sp_EliminarCliente
   - sp_AsignarPuntosCliente

7. Usuarios: 5
   - sp_ListarUsuarios
   - sp_ObtenerUsuarioPorId
   - sp_InsertarUsuario
   - sp_ActualizarUsuario
   - sp_EliminarUsuario

8. Compras: 5
   - sp_InsertarCompra
   - sp_InsertarDetalleCompra
   - sp_ListarDetallesPorCompra
   - sp_ActualizarDetalleCompra
   - sp_EliminarDetalleCompra

9. Lotes: 4
   - sp_InsertarLote
   - sp_ListarLotesPorProducto
   - sp_ObtenerLotePorId
   - sp_MarcarLotesVencidos

10. Sesiones: 3
    - sp_ListarSesionesActivas
    - sp_AbrirSesion
    - sp_CerrarSesion

11. Facturas: 4
    - sp_InsertarFactura
    - sp_InsertarDetalleFactura
    - sp_ListarDetallesPorFactura
    - sp_AnularFactura

12. Reportes: 4
    - sp_ReporteVentas
    - sp_Top5ProductosMasVendidos
    - sp_Top5CategoriasMasVendidas
    - sp_Top5ProductosMasIngresos

13. Vistas: 1
    - vw_InventarioGeneral (vista, no SP)
*/

