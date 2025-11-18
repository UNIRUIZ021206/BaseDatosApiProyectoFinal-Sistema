-- =============================================
-- CREACIÓN DE TODOS LOS STORED PROCEDURES
-- =============================================
-- Este archivo contiene las definiciones CREATE PROCEDURE
-- para todos los stored procedures que utiliza la API SuperEsperanza
-- =============================================

USE [SuperLaEsperanzaDB]
GO

-- =============================================
-- 1. AUTENTICACIÓN
-- =============================================

-- sp_ValidarUsuario_Super
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ValidarUsuario_Super]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ValidarUsuario_Super]
GO

CREATE PROCEDURE [dbo].[sp_ValidarUsuario_Super]
    @CodigoUsuario NVARCHAR(50),
    @Contrasena NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.Id_Usuario AS IdUsuario,
        u.Nombre + ' ' + u.Apellido AS NombreUsuario,
        r.NombreRol AS NombreRol
    FROM Usuario u
    INNER JOIN Rol r ON u.Id_Rol = r.Id_Rol
    WHERE u.CodigoUsuario = @CodigoUsuario
        AND u.Clave = HASHBYTES('SHA2_256', @Contrasena)
        AND u.Estado = 1;
END
GO

-- =============================================
-- 2. ROLES
-- =============================================

-- sp_ListarRoles
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarRoles]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarRoles]
GO

CREATE PROCEDURE [dbo].[sp_ListarRoles]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Rol,
        NombreRol
    FROM Rol
    ORDER BY NombreRol;
END
GO

-- sp_ObtenerRolPorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerRolPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerRolPorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerRolPorId]
    @Id_Rol INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Rol,
        NombreRol,
        FechaCreacion,
        FechaModificacion
    FROM Rol
    WHERE Id_Rol = @Id_Rol;
END
GO

-- sp_InsertarRol
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarRol]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarRol]
GO

CREATE PROCEDURE [dbo].[sp_InsertarRol]
    @NombreRol VARCHAR(50),
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Rol (NombreRol, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@NombreRol, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Rol;
END
GO

-- sp_ActualizarRol
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarRol]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarRol]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarRol]
    @Id_Rol INT,
    @NombreRol VARCHAR(50),
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Rol
    SET NombreRol = @NombreRol,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Rol = @Id_Rol;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarRol
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarRol]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarRol]
GO

CREATE PROCEDURE [dbo].[sp_EliminarRol]
    @Id_Rol INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si hay usuarios con este rol
    IF EXISTS (SELECT 1 FROM Usuario WHERE Id_Rol = @Id_Rol)
    BEGIN
        RAISERROR('No se puede eliminar el rol porque tiene usuarios asignados', 16, 1);
        RETURN -1;
    END
    
    DELETE FROM Rol WHERE Id_Rol = @Id_Rol;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 3. CATEGORÍAS
-- =============================================

-- sp_ListarCategorias
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarCategorias]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarCategorias]
GO

CREATE PROCEDURE [dbo].[sp_ListarCategorias]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Categoria,
        CodigoCategoria,
        Nombre,
        Descripcion,
        Estado,
        FechaCreacion,
        Id_UsuarioCreacion
    FROM Categoria
    ORDER BY Nombre;
END
GO

-- sp_ObtenerCategoriaPorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerCategoriaPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerCategoriaPorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerCategoriaPorId]
    @Id_Categoria INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Categoria,
        CodigoCategoria,
        Nombre,
        Descripcion,
        Estado,
        FechaCreacion,
        Id_UsuarioCreacion
    FROM Categoria
    WHERE Id_Categoria = @Id_Categoria;
END
GO

-- sp_InsertarCategoria
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarCategoria]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarCategoria]
GO

CREATE PROCEDURE [dbo].[sp_InsertarCategoria]
    @CodigoCategoria VARCHAR(10),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500) = NULL,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Categoria (CodigoCategoria, Nombre, Descripcion, Estado, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoCategoria, @Nombre, @Descripcion, 1, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Categoria;
END
GO

-- sp_ActualizarCategoria
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarCategoria]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarCategoria]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarCategoria]
    @Id_Categoria INT,
    @CodigoCategoria VARCHAR(10),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500) = NULL,
    @Estado BIT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Categoria
    SET CodigoCategoria = @CodigoCategoria,
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Categoria = @Id_Categoria;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarCategoria
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarCategoria]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarCategoria]
GO

CREATE PROCEDURE [dbo].[sp_EliminarCategoria]
    @Id_Categoria INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si hay productos con esta categoría
    IF EXISTS (SELECT 1 FROM Producto WHERE Id_Categoria = @Id_Categoria)
    BEGIN
        RAISERROR('No se puede eliminar la categoría porque tiene productos asignados', 16, 1);
        RETURN -1;
    END
    
    UPDATE Categoria
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Categoria = @Id_Categoria;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 4. PRODUCTOS
-- =============================================

-- sp_ListarProductos
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarProductos]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarProductos]
GO

CREATE PROCEDURE [dbo].[sp_ListarProductos]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id_Producto,
        p.CodigoProducto,
        p.Id_Categoria,
        c.Nombre AS NombreCategoria,
        p.Nombre,
        p.Descripcion,
        p.PrecioVenta,
        p.StockActual,
        p.CostoPromedio,
        p.Estado,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Id_UsuarioCreacion,
        p.Id_UsuarioModificacion
    FROM Producto p
    LEFT JOIN Categoria c ON p.Id_Categoria = c.Id_Categoria
    ORDER BY p.Nombre;
END
GO

-- sp_ObtenerProductoPorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerProductoPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerProductoPorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerProductoPorId]
    @Id_Producto INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id_Producto,
        p.CodigoProducto,
        p.Id_Categoria,
        c.Nombre AS NombreCategoria,
        p.Nombre,
        p.Descripcion,
        p.PrecioVenta,
        p.StockActual,
        p.CostoPromedio,
        p.Estado,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Id_UsuarioCreacion,
        p.Id_UsuarioModificacion
    FROM Producto p
    LEFT JOIN Categoria c ON p.Id_Categoria = c.Id_Categoria
    WHERE p.Id_Producto = @Id_Producto;
END
GO

-- sp_InsertarProducto
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarProducto]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarProducto]
GO

CREATE PROCEDURE [dbo].[sp_InsertarProducto]
    @CodigoProducto VARCHAR(10),
    @Id_Categoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500) = NULL,
    @PrecioVenta DECIMAL(18,2),
    @StockActual INT,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Producto (CodigoProducto, Id_Categoria, Nombre, Descripcion, PrecioVenta, StockActual, Estado, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoProducto, @Id_Categoria, @Nombre, @Descripcion, @PrecioVenta, @StockActual, 1, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Producto;
END
GO

-- sp_ActualizarProducto
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarProducto]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarProducto]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarProducto]
    @Id_Producto INT,
    @CodigoProducto VARCHAR(10),
    @Id_Categoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500) = NULL,
    @PrecioVenta DECIMAL(18,2),
    @StockActual INT,
    @Estado BIT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Producto
    SET CodigoProducto = @CodigoProducto,
        Id_Categoria = @Id_Categoria,
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        PrecioVenta = @PrecioVenta,
        StockActual = @StockActual,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Producto = @Id_Producto;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarProducto
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarProducto]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarProducto]
GO

CREATE PROCEDURE [dbo].[sp_EliminarProducto]
    @Id_Producto INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Producto
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Producto = @Id_Producto;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 5. PROVEEDORES
-- =============================================

-- sp_ListarProveedores
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarProveedores]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarProveedores]
GO

CREATE PROCEDURE [dbo].[sp_ListarProveedores]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Proveedor,
        CodigoProveedor,
        Nombre,
        Telefono,
        Email,
        Contacto,
        Estado,
        FechaCreacion,
        FechaModificacion,
        Id_UsuarioCreacion,
        Id_UsuarioModificacion
    FROM Proveedor
    ORDER BY Nombre;
END
GO

-- sp_ObtenerProveedorPorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerProveedorPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerProveedorPorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerProveedorPorId]
    @Id_Proveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Proveedor,
        CodigoProveedor,
        Nombre,
        Telefono,
        Email,
        Contacto,
        Estado,
        FechaCreacion,
        FechaModificacion,
        Id_UsuarioCreacion,
        Id_UsuarioModificacion
    FROM Proveedor
    WHERE Id_Proveedor = @Id_Proveedor;
END
GO

-- sp_InsertarProveedor
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarProveedor]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarProveedor]
GO

CREATE PROCEDURE [dbo].[sp_InsertarProveedor]
    @CodigoProveedor VARCHAR(10),
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15) = NULL,
    @Email VARCHAR(100) = NULL,
    @Contacto VARCHAR(100) = NULL,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Proveedor (CodigoProveedor, Nombre, Telefono, Email, Contacto, Estado, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoProveedor, @Nombre, @Telefono, @Email, @Contacto, 1, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Proveedor;
END
GO

-- sp_ActualizarProveedor
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarProveedor]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarProveedor]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarProveedor]
    @Id_Proveedor INT,
    @CodigoProveedor VARCHAR(10),
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15) = NULL,
    @Email VARCHAR(100) = NULL,
    @Contacto VARCHAR(100) = NULL,
    @Estado BIT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Proveedor
    SET CodigoProveedor = @CodigoProveedor,
        Nombre = @Nombre,
        Telefono = @Telefono,
        Email = @Email,
        Contacto = @Contacto,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Proveedor = @Id_Proveedor;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarProveedor
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarProveedor]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarProveedor]
GO

CREATE PROCEDURE [dbo].[sp_EliminarProveedor]
    @Id_Proveedor INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Proveedor
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Proveedor = @Id_Proveedor;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 6. CLIENTES
-- =============================================

-- sp_ListarClientes
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarClientes]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarClientes]
GO

CREATE PROCEDURE [dbo].[sp_ListarClientes]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Cliente,
        CodigoCliente,
        P_Nombre,
        S_Nombre,
        P_Apellido,
        S_Apellido,
        Telefono,
        Direccion,
        Email,
        TipoMembresia,
        Estado,
        FechaRegistro,
        PuntosCompra,
        UltimaCompra,
        Id_UsuarioCreacion,
        Id_UsuarioModificacion,
        FechaModificacion
    FROM Cliente
    ORDER BY P_Apellido, P_Nombre;
END
GO

-- sp_ObtenerClientePorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerClientePorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerClientePorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerClientePorId]
    @Id_Cliente INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Cliente,
        CodigoCliente,
        P_Nombre,
        S_Nombre,
        P_Apellido,
        S_Apellido,
        Telefono,
        Direccion,
        Email,
        TipoMembresia,
        Estado,
        FechaRegistro,
        PuntosCompra,
        UltimaCompra,
        Id_UsuarioCreacion,
        Id_UsuarioModificacion,
        FechaModificacion
    FROM Cliente
    WHERE Id_Cliente = @Id_Cliente;
END
GO

-- sp_InsertarCliente
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarCliente]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarCliente]
GO

CREATE PROCEDURE [dbo].[sp_InsertarCliente]
    @CodigoCliente VARCHAR(10),
    @P_Nombre VARCHAR(50),
    @S_Nombre VARCHAR(50) = NULL,
    @P_Apellido VARCHAR(50),
    @S_Apellido VARCHAR(50) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Direccion VARCHAR(200) = NULL,
    @Email VARCHAR(100) = NULL,
    @TipoMembresia VARCHAR(20) = NULL,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Cliente (CodigoCliente, P_Nombre, S_Nombre, P_Apellido, S_Apellido, Telefono, Direccion, Email, TipoMembresia, Estado, FechaRegistro, Id_UsuarioCreacion)
    VALUES (@CodigoCliente, @P_Nombre, @S_Nombre, @P_Apellido, @S_Apellido, @Telefono, @Direccion, @Email, @TipoMembresia, 1, CAST(GETDATE() AS DATE), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Cliente;
END
GO

-- sp_ActualizarCliente
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarCliente]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarCliente]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarCliente]
    @Id_Cliente INT,
    @CodigoCliente VARCHAR(10),
    @P_Nombre VARCHAR(50),
    @S_Nombre VARCHAR(50) = NULL,
    @P_Apellido VARCHAR(50),
    @S_Apellido VARCHAR(50) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Direccion VARCHAR(200) = NULL,
    @Email VARCHAR(100) = NULL,
    @TipoMembresia VARCHAR(20) = NULL,
    @Estado BIT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Cliente
    SET CodigoCliente = @CodigoCliente,
        P_Nombre = @P_Nombre,
        S_Nombre = @S_Nombre,
        P_Apellido = @P_Apellido,
        S_Apellido = @S_Apellido,
        Telefono = @Telefono,
        Direccion = @Direccion,
        Email = @Email,
        TipoMembresia = @TipoMembresia,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Cliente = @Id_Cliente;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarCliente
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarCliente]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarCliente]
GO

CREATE PROCEDURE [dbo].[sp_EliminarCliente]
    @Id_Cliente INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Cliente
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Cliente = @Id_Cliente;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_AsignarPuntosCliente
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AsignarPuntosCliente]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_AsignarPuntosCliente]
GO

CREATE PROCEDURE [dbo].[sp_AsignarPuntosCliente]
    @Id_Cliente INT,
    @MontoTotalCompra DECIMAL(18,2),
    @Id_UsuarioOperacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @PuntosGanados INT;
    DECLARE @NuevaMembresia VARCHAR(20);
    DECLARE @PuntosActuales INT;
    DECLARE @PuntosTotales INT;
    
    -- Calcular puntos ganados (1 punto por cada 10 unidades de compra)
    SET @PuntosGanados = CAST(@MontoTotalCompra / 10 AS INT);
    
    -- Obtener puntos actuales
    SELECT @PuntosActuales = ISNULL(PuntosCompra, 0) FROM Cliente WHERE Id_Cliente = @Id_Cliente;
    
    -- Calcular puntos totales
    SET @PuntosTotales = @PuntosActuales + @PuntosGanados;
    
    -- Determinar membresía según puntos totales
    -- Valores válidos según esquema: 'BASICA', 'PREMIUM', 'VIP', 'PLATINUM'
    IF @PuntosTotales >= 1000
        SET @NuevaMembresia = 'PLATINUM'
    ELSE IF @PuntosTotales >= 500
        SET @NuevaMembresia = 'VIP'
    ELSE IF @PuntosTotales >= 100
        SET @NuevaMembresia = 'PREMIUM'
    ELSE
        SET @NuevaMembresia = 'BASICA'
    
    -- Actualizar puntos y membresía
    UPDATE Cliente
    SET PuntosCompra = @PuntosTotales,
        TipoMembresia = @NuevaMembresia,
        UltimaCompra = GETDATE(),
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioOperacion
    WHERE Id_Cliente = @Id_Cliente;
    
    -- Retornar resultados
    SELECT 
        @PuntosGanados AS PuntosGanadosEnEstaCompra,
        @NuevaMembresia AS Membresia;
END
GO

-- =============================================
-- 7. USUARIOS (CRUD)
-- =============================================

-- sp_ListarUsuarios
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarUsuarios]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarUsuarios]
GO

CREATE PROCEDURE [dbo].[sp_ListarUsuarios]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.Id_Usuario,
        u.CodigoUsuario,
        u.Nombre,
        u.Apellido,
        u.Email,
        u.Estado,
        r.NombreRol
    FROM Usuario u
    INNER JOIN Rol r ON u.Id_Rol = r.Id_Rol
    ORDER BY u.Apellido, u.Nombre;
END
GO

-- sp_ObtenerUsuarioPorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerUsuarioPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerUsuarioPorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerUsuarioPorId]
    @Id_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Usuario,
        CodigoUsuario,
        Nombre,
        Apellido,
        Email,
        Telefono,
        Id_Rol,
        Estado
    FROM Usuario
    WHERE Id_Usuario = @Id_Usuario;
END
GO

-- sp_InsertarUsuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarUsuario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarUsuario]
GO

CREATE PROCEDURE [dbo].[sp_InsertarUsuario]
    @CodigoUsuario VARCHAR(10),
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @Email VARCHAR(100) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Clave VARCHAR(100),
    @Id_Rol INT,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Usuario (CodigoUsuario, Nombre, Apellido, Email, Telefono, Clave, Id_Rol, Estado, Id_UsuarioCreacion)
    VALUES (@CodigoUsuario, @Nombre, @Apellido, @Email, @Telefono, HASHBYTES('SHA2_256', @Clave), @Id_Rol, 1, @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Usuario;
END
GO

-- sp_ActualizarUsuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarUsuario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarUsuario]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarUsuario]
    @Id_Usuario INT,
    @CodigoUsuario VARCHAR(10),
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @Email VARCHAR(100) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Clave VARCHAR(100) = NULL,
    @Id_Rol INT,
    @Estado BIT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Clave IS NOT NULL AND @Clave != ''
    BEGIN
        UPDATE Usuario
        SET CodigoUsuario = @CodigoUsuario,
            Nombre = @Nombre,
            Apellido = @Apellido,
            Email = @Email,
            Telefono = @Telefono,
            Clave = HASHBYTES('SHA2_256', @Clave),
            Id_Rol = @Id_Rol,
            Estado = @Estado,
            FechaModificacion = GETDATE(),
            Id_UsuarioModificacion = @Id_UsuarioModificacion
        WHERE Id_Usuario = @Id_Usuario;
    END
    ELSE
    BEGIN
        UPDATE Usuario
        SET CodigoUsuario = @CodigoUsuario,
            Nombre = @Nombre,
            Apellido = @Apellido,
            Email = @Email,
            Telefono = @Telefono,
            Id_Rol = @Id_Rol,
            Estado = @Estado,
            FechaModificacion = GETDATE(),
            Id_UsuarioModificacion = @Id_UsuarioModificacion
        WHERE Id_Usuario = @Id_Usuario;
    END
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarUsuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarUsuario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarUsuario]
GO

CREATE PROCEDURE [dbo].[sp_EliminarUsuario]
    @Id_Usuario INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Usuario
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Usuario = @Id_Usuario;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 8. COMPRAS
-- =============================================

-- sp_InsertarCompra
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarCompra]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarCompra]
GO

CREATE PROCEDURE [dbo].[sp_InsertarCompra]
    @CodigoCompra VARCHAR(10),
    @Id_Proveedor INT,
    @FechaCompra DATE,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Compra (CodigoCompra, Id_Proveedor, FechaCompra, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoCompra, @Id_Proveedor, @FechaCompra, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Compra;
END
GO

-- sp_InsertarDetalleCompra
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarDetalleCompra]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarDetalleCompra]
GO

CREATE PROCEDURE [dbo].[sp_InsertarDetalleCompra]
    @Id_Compra INT,
    @Id_Producto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO DetalleCompra (Id_Compra, Id_Producto, Cantidad, PrecioUnitario, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@Id_Compra, @Id_Producto, @Cantidad, @PrecioUnitario, GETDATE(), @Id_UsuarioCreacion);
    
    -- Actualizar stock del producto
    UPDATE Producto
    SET StockActual = StockActual + @Cantidad,
        CostoPromedio = (
            SELECT AVG(PrecioUnitario)
            FROM DetalleCompra
            WHERE Id_Producto = @Id_Producto
        )
    WHERE Id_Producto = @Id_Producto;
END
GO

-- sp_ListarDetallesPorCompra
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarDetallesPorCompra]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarDetallesPorCompra]
GO

CREATE PROCEDURE [dbo].[sp_ListarDetallesPorCompra]
    @Id_Compra INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        dc.Id_DetalleCompra,
        dc.Id_Compra,
        dc.Id_Producto,
        p.Nombre AS NombreProducto,
        dc.Cantidad,
        dc.PrecioUnitario,
        (dc.Cantidad * dc.PrecioUnitario) AS Subtotal
    FROM DetalleCompra dc
    INNER JOIN Producto p ON dc.Id_Producto = p.Id_Producto
    WHERE dc.Id_Compra = @Id_Compra
    ORDER BY dc.Id_DetalleCompra;
END
GO

-- sp_ActualizarDetalleCompra
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ActualizarDetalleCompra]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ActualizarDetalleCompra]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarDetalleCompra]
    @Id_DetalleCompra INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CantidadAnterior INT;
    DECLARE @Id_Producto INT;
    
    -- Obtener cantidad anterior y producto
    SELECT @CantidadAnterior = Cantidad, @Id_Producto = Id_Producto
    FROM DetalleCompra
    WHERE Id_DetalleCompra = @Id_DetalleCompra;
    
    -- Actualizar detalle
    UPDATE DetalleCompra
    SET Cantidad = @Cantidad,
        PrecioUnitario = @PrecioUnitario,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_DetalleCompra = @Id_DetalleCompra;
    
    -- Ajustar stock del producto
    UPDATE Producto
    SET StockActual = StockActual - @CantidadAnterior + @Cantidad
    WHERE Id_Producto = @Id_Producto;
    
    SELECT @@ROWCOUNT;
END
GO

-- sp_EliminarDetalleCompra
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarDetalleCompra]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarDetalleCompra]
GO

CREATE PROCEDURE [dbo].[sp_EliminarDetalleCompra]
    @Id_DetalleCompra INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Cantidad INT;
    DECLARE @Id_Producto INT;
    
    -- Obtener cantidad y producto antes de eliminar
    SELECT @Cantidad = Cantidad, @Id_Producto = Id_Producto
    FROM DetalleCompra
    WHERE Id_DetalleCompra = @Id_DetalleCompra;
    
    -- Eliminar detalle
    DELETE FROM DetalleCompra WHERE Id_DetalleCompra = @Id_DetalleCompra;
    
    -- Ajustar stock del producto
    UPDATE Producto
    SET StockActual = StockActual - @Cantidad
    WHERE Id_Producto = @Id_Producto;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 9. LOTES
-- =============================================

-- sp_InsertarLote
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarLote]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarLote]
GO

CREATE PROCEDURE [dbo].[sp_InsertarLote]
    @CodigoLote VARCHAR(10),
    @Id_Producto INT,
    @Id_Compra INT,
    @Cantidad INT,
    @FechaVencimiento DATE = NULL,
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Lote (CodigoLote, Id_Producto, Id_Compra, Cantidad, FechaIngreso, FechaVencimiento, Estado, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoLote, @Id_Producto, @Id_Compra, @Cantidad, CAST(GETDATE() AS DATE), @FechaVencimiento, 1, GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Lote;
END
GO

-- sp_ListarLotesPorProducto
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarLotesPorProducto]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarLotesPorProducto]
GO

CREATE PROCEDURE [dbo].[sp_ListarLotesPorProducto]
    @Id_Producto INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Lote,
        CodigoLote,
        Id_Producto,
        Cantidad,
        FechaIngreso,
        FechaVencimiento,
        Estado
    FROM Lote
    WHERE Id_Producto = @Id_Producto AND Estado = 1
    ORDER BY FechaIngreso DESC;
END
GO

-- sp_ObtenerLotePorId
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerLotePorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerLotePorId]
GO

CREATE PROCEDURE [dbo].[sp_ObtenerLotePorId]
    @Id_Lote INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id_Lote,
        CodigoLote,
        Id_Producto,
        Id_Compra,
        Cantidad,
        FechaIngreso,
        FechaVencimiento,
        Estado,
        FechaCreacion,
        Id_UsuarioCreacion
    FROM Lote
    WHERE Id_Lote = @Id_Lote;
END
GO

-- sp_MarcarLotesVencidos
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_MarcarLotesVencidos]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_MarcarLotesVencidos]
GO

CREATE PROCEDURE [dbo].[sp_MarcarLotesVencidos]
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Lote
    SET Estado = 0
    WHERE FechaVencimiento IS NOT NULL
        AND FechaVencimiento < GETDATE()
        AND Estado = 1;
END
GO

-- =============================================
-- 10. SESIONES
-- =============================================

-- sp_ListarSesionesActivas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarSesionesActivas]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarSesionesActivas]
GO

CREATE PROCEDURE [dbo].[sp_ListarSesionesActivas]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.Id_Sesion,
        s.CodigoSesion,
        u.Nombre + ' ' + u.Apellido AS UsuarioNombre,
        s.FechaApertura,
        s.MontoInicial,
        s.Estado
    FROM Sesion s
    INNER JOIN Usuario u ON s.Id_Usuario = u.Id_Usuario
    WHERE s.Estado = 1
    ORDER BY s.FechaApertura DESC;
END
GO

-- sp_AbrirSesion
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AbrirSesion]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_AbrirSesion]
GO

CREATE PROCEDURE [dbo].[sp_AbrirSesion]
    @CodigoSesion VARCHAR(10),
    @Id_Usuario INT,
    @MontoInicial DECIMAL(18,2),
    @Observacion VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Sesion (CodigoSesion, Id_Usuario, FechaApertura, MontoInicial, Observacion, Estado, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoSesion, @Id_Usuario, GETDATE(), @MontoInicial, @Observacion, 1, GETDATE(), @Id_Usuario);
    
    SELECT SCOPE_IDENTITY() AS Id_Sesion;
END
GO

-- sp_CerrarSesion
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CerrarSesion]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_CerrarSesion]
GO

CREATE PROCEDURE [dbo].[sp_CerrarSesion]
    @Id_Sesion INT,
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Sesion
    SET Estado = 0,
        FechaModificacion = GETDATE(),
        Id_UsuarioModificacion = @Id_UsuarioModificacion
    WHERE Id_Sesion = @Id_Sesion AND Estado = 1;
    
    SELECT @@ROWCOUNT;
END
GO

-- =============================================
-- 11. FACTURAS
-- =============================================

-- sp_InsertarFactura
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarFactura]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarFactura]
GO

CREATE PROCEDURE [dbo].[sp_InsertarFactura]
    @CodigoFactura VARCHAR(10),
    @Id_Cliente INT,
    @Id_Sesion INT,
    @NumeroFactura VARCHAR(20),
    @Subtotal DECIMAL(18,2),
    @Descuento DECIMAL(18,2),
    @Impuesto DECIMAL(18,2),
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- El campo Total es un campo calculado (PERSISTED) en la tabla, no se inserta
    INSERT INTO Factura (CodigoFactura, Id_Cliente, Id_Sesion, NumeroFactura, Subtotal, Descuento, Impuesto, EstadoFactura, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@CodigoFactura, @Id_Cliente, @Id_Sesion, @NumeroFactura, @Subtotal, @Descuento, @Impuesto, 'ACTIVA', GETDATE(), @Id_UsuarioCreacion);
    
    SELECT SCOPE_IDENTITY() AS Id_Factura;
END
GO

-- sp_InsertarDetalleFactura
-- NOTA: Este stored procedure ya existe en sp_InsertarDetalleFactura_Mejorado.sql
-- Se recomienda usar esa versión que incluye validaciones y locks
-- Aquí se incluye una versión básica como referencia

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarDetalleFactura]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarDetalleFactura]
GO

-- NOTA: Para la versión completa con validaciones, usar el archivo: sp_InsertarDetalleFactura_Mejorado.sql
-- Esta es una versión simplificada
CREATE PROCEDURE [dbo].[sp_InsertarDetalleFactura]
    @Id_Factura INT,
    @Id_Lote INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    DECLARE @StockDisponible INT;
    DECLARE @EstadoLote BIT;
    
    -- Validar lote y obtener stock
    SELECT @StockDisponible = Cantidad, @EstadoLote = Estado
    FROM Lote WITH (UPDLOCK, ROWLOCK)
    WHERE Id_Lote = @Id_Lote;
    
    IF @StockDisponible IS NULL
    BEGIN
        RAISERROR('El lote especificado no existe', 16, 1);
        RETURN -1;
    END
    
    IF @EstadoLote = 0
    BEGIN
        RAISERROR('El lote no está activo', 16, 1);
        RETURN -1;
    END
    
    IF @Cantidad <= 0 OR @PrecioUnitario <= 0
    BEGIN
        RAISERROR('La cantidad y el precio deben ser mayores a cero', 16, 1);
        RETURN -1;
    END
    
    IF @Cantidad > @StockDisponible
    BEGIN
        RAISERROR('Stock insuficiente. Disponible: %d, Solicitado: %d', 16, 1, @StockDisponible, @Cantidad);
        RETURN -1;
    END
    
    -- Insertar detalle
    INSERT INTO DetalleFactura (Id_Factura, Id_Lote, Cantidad, PrecioUnitario, FechaCreacion, Id_UsuarioCreacion)
    VALUES (@Id_Factura, @Id_Lote, @Cantidad, @PrecioUnitario, GETDATE(), @Id_UsuarioCreacion);
    
    -- Actualizar stock del lote
    UPDATE Lote
    SET Cantidad = Cantidad - @Cantidad
    WHERE Id_Lote = @Id_Lote;
    
    -- Actualizar stock del producto
    UPDATE Producto
    SET StockActual = StockActual - @Cantidad
    WHERE Id_Producto = (SELECT Id_Producto FROM Lote WHERE Id_Lote = @Id_Lote);
    
    SELECT 1;
END
GO

-- sp_ListarDetallesPorFactura
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ListarDetallesPorFactura]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ListarDetallesPorFactura]
GO

CREATE PROCEDURE [dbo].[sp_ListarDetallesPorFactura]
    @Id_Factura INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        df.Id_Detalle AS Id_Detalle,
        df.Id_Factura,
        df.Id_Lote,
        l.CodigoLote,
        p.Nombre AS NombreProducto,
        df.Cantidad,
        df.PrecioUnitario,
        (df.Cantidad * df.PrecioUnitario) AS Subtotal
    FROM DetalleFactura df
    INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
    INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
    WHERE df.Id_Factura = @Id_Factura
    ORDER BY df.Id_Detalle;
END
GO

-- sp_AnularFactura
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AnularFactura]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_AnularFactura]
GO

CREATE PROCEDURE [dbo].[sp_AnularFactura]
    @Id_Factura INT,
    @Motivo VARCHAR(500),
    @Id_UsuarioModificacion INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    DECLARE @EstadoActual VARCHAR(20);
    
    -- Verificar que la factura existe y obtener estado
    SELECT @EstadoActual = EstadoFactura
    FROM Factura
    WHERE Id_Factura = @Id_Factura;
    
    IF @EstadoActual IS NULL
    BEGIN
        RAISERROR('La factura especificada no existe', 16, 1);
        RETURN -1;
    END
    
    IF @EstadoActual = 'ANULADA'
    BEGIN
        RAISERROR('La factura ya está anulada', 16, 1);
        RETURN -1;
    END
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Restaurar stock de los lotes
        UPDATE l
        SET l.Cantidad = l.Cantidad + df.Cantidad
        FROM Lote l
        INNER JOIN DetalleFactura df ON l.Id_Lote = df.Id_Lote
        WHERE df.Id_Factura = @Id_Factura;
        
        -- Restaurar stock de productos
        UPDATE p
        SET p.StockActual = p.StockActual + df.Cantidad
        FROM Producto p
        INNER JOIN Lote l ON p.Id_Producto = l.Id_Producto
        INNER JOIN DetalleFactura df ON l.Id_Lote = df.Id_Lote
        WHERE df.Id_Factura = @Id_Factura;
        
        -- Anular factura
        UPDATE Factura
        SET EstadoFactura = 'ANULADA',
            FechaAnulacion = GETDATE(),
            MotivoAnulacion = @Motivo,
            FechaModificacion = GETDATE(),
            Id_UsuarioModificacion = @Id_UsuarioModificacion
        WHERE Id_Factura = @Id_Factura;
        
        COMMIT TRANSACTION;
        SELECT 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =============================================
-- 12. REPORTES
-- =============================================

-- sp_ReporteVentas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ReporteVentas]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ReporteVentas]
GO

CREATE PROCEDURE [dbo].[sp_ReporteVentas]
    @Fecha DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Nombre AS Producto,
        SUM(df.Cantidad) AS CantidadVendida,
        SUM(df.Cantidad * df.PrecioUnitario) AS TotalVenta
    FROM DetalleFactura df
    INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
    INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
    INNER JOIN Factura f ON df.Id_Factura = f.Id_Factura
    WHERE CAST(f.FechaCreacion AS DATE) = @Fecha
        AND f.EstadoFactura = 'ACTIVA'
    GROUP BY p.Id_Producto, p.Nombre
    ORDER BY TotalVenta DESC;
END
GO

-- sp_Top5ProductosMasVendidos
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Top5ProductosMasVendidos]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_Top5ProductosMasVendidos]
GO

CREATE PROCEDURE [dbo].[sp_Top5ProductosMasVendidos]
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no se proporcionan fechas, usar todos los registros
    IF @FechaInicio IS NULL
        SET @FechaInicio = '1900-01-01';
    IF @FechaFin IS NULL
        SET @FechaFin = GETDATE();
    
    SELECT TOP 5
        p.Id_Producto,
        p.CodigoProducto,
        p.Nombre AS Producto,
        c.Nombre AS Categoria,
        SUM(df.Cantidad) AS CantidadVendida,
        SUM(df.Cantidad * df.PrecioUnitario) AS TotalIngresos,
        COUNT(DISTINCT df.Id_Factura) AS NumeroFacturas
    FROM DetalleFactura df
    INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
    INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
    LEFT JOIN Categoria c ON p.Id_Categoria = c.Id_Categoria
    INNER JOIN Factura f ON df.Id_Factura = f.Id_Factura
    WHERE CAST(f.FechaCreacion AS DATE) BETWEEN @FechaInicio AND @FechaFin
        AND f.EstadoFactura = 'ACTIVA'
    GROUP BY p.Id_Producto, p.CodigoProducto, p.Nombre, c.Nombre
    ORDER BY CantidadVendida DESC;
END
GO

-- sp_Top5CategoriasMasVendidas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Top5CategoriasMasVendidas]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_Top5CategoriasMasVendidas]
GO

CREATE PROCEDURE [dbo].[sp_Top5CategoriasMasVendidas]
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no se proporcionan fechas, usar todos los registros
    IF @FechaInicio IS NULL
        SET @FechaInicio = '1900-01-01';
    IF @FechaFin IS NULL
        SET @FechaFin = GETDATE();
    
    SELECT TOP 5
        c.Id_Categoria,
        c.CodigoCategoria,
        c.Nombre AS Categoria,
        SUM(df.Cantidad) AS CantidadVendida,
        SUM(df.Cantidad * df.PrecioUnitario) AS TotalIngresos,
        COUNT(DISTINCT p.Id_Producto) AS NumeroProductos,
        COUNT(DISTINCT df.Id_Factura) AS NumeroFacturas
    FROM DetalleFactura df
    INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
    INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
    INNER JOIN Categoria c ON p.Id_Categoria = c.Id_Categoria
    INNER JOIN Factura f ON df.Id_Factura = f.Id_Factura
    WHERE CAST(f.FechaCreacion AS DATE) BETWEEN @FechaInicio AND @FechaFin
        AND f.EstadoFactura = 'ACTIVA'
    GROUP BY c.Id_Categoria, c.CodigoCategoria, c.Nombre
    ORDER BY CantidadVendida DESC;
END
GO

-- sp_Top5ProductosMasIngresos
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Top5ProductosMasIngresos]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_Top5ProductosMasIngresos]
GO

CREATE PROCEDURE [dbo].[sp_Top5ProductosMasIngresos]
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no se proporcionan fechas, usar todos los registros
    IF @FechaInicio IS NULL
        SET @FechaInicio = '1900-01-01';
    IF @FechaFin IS NULL
        SET @FechaFin = GETDATE();
    
    SELECT TOP 5
        p.Id_Producto,
        p.CodigoProducto,
        p.Nombre AS Producto,
        c.Nombre AS Categoria,
        SUM(df.Cantidad) AS CantidadVendida,
        SUM(df.Cantidad * df.PrecioUnitario) AS TotalIngresos,
        AVG(df.PrecioUnitario) AS PrecioPromedio,
        COUNT(DISTINCT df.Id_Factura) AS NumeroFacturas
    FROM DetalleFactura df
    INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
    INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
    LEFT JOIN Categoria c ON p.Id_Categoria = c.Id_Categoria
    INNER JOIN Factura f ON df.Id_Factura = f.Id_Factura
    WHERE CAST(f.FechaCreacion AS DATE) BETWEEN @FechaInicio AND @FechaFin
        AND f.EstadoFactura = 'ACTIVA'
    GROUP BY p.Id_Producto, p.CodigoProducto, p.Nombre, c.Nombre
    ORDER BY TotalIngresos DESC;
END
GO

-- =============================================
-- FIN DEL SCRIPT
-- =============================================
PRINT 'Todos los stored procedures han sido creados exitosamente';
GO

