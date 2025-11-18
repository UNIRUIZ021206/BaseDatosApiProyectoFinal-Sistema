-- =============================================
-- Script de Prueba para sp_InsertarDetalleFactura
-- Simula una venta de 13 unidades usando dos lotes (210: 4 unidades, 211: 9 unidades)
-- =============================================

-- Paso 1: Verificar el stock actual de los lotes antes de la prueba
PRINT '=== STOCK ANTES DE LA PRUEBA ===';
SELECT 
    Id_Lote,
    CodigoLote,
    Cantidad AS StockActual,
    Estado,
    FechaVencimiento
FROM Lote
WHERE Id_Lote IN (210, 211)
ORDER BY Id_Lote;
GO

-- Paso 2: Crear una factura de prueba (o usar una existente)
-- Nota: Ajusta estos valores según tu base de datos
DECLARE @Id_Factura INT;
DECLARE @Id_Sesion INT;
DECLARE @Id_UsuarioCreacion INT = 1; -- Ajusta según tu usuario
DECLARE @Id_Usuario INT = 1; -- Ajusta según tu usuario (puede ser el mismo que Id_UsuarioCreacion)
DECLARE @CodigoFactura VARCHAR(10) = 'TEST' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(6));
DECLARE @CodigoSesion VARCHAR(10);

-- Obtener una sesión activa (ajusta según tu lógica)
-- Estado = 1 significa Abierta, Estado = 0 significa Cerrada
SELECT TOP 1 @Id_Sesion = Id_Sesion 
FROM Sesion 
WHERE Estado = 1  -- 1 = Abierta
ORDER BY FechaApertura DESC;

-- Si no hay sesión activa, crear una de prueba
IF @Id_Sesion IS NULL
BEGIN
    SET @Id_Usuario = @Id_UsuarioCreacion; -- Usar el mismo usuario
    SET @CodigoSesion = 'TEST' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(6));
    
    INSERT INTO Sesion (
        CodigoSesion, 
        Id_Usuario, 
        FechaApertura, 
        MontoInicial, 
        Estado, 
        Id_UsuarioCreacion, 
        FechaCreacion
    )
    VALUES (
        @CodigoSesion, 
        @Id_Usuario, 
        GETDATE(), 
        0.00,  -- Monto inicial de prueba
        1,     -- 1 = Abierta
        @Id_UsuarioCreacion, 
        GETDATE()
    );
    SET @Id_Sesion = SCOPE_IDENTITY();
    PRINT 'Sesión de prueba creada: ID ' + CAST(@Id_Sesion AS VARCHAR(10)) + ', Código: ' + @CodigoSesion;
END
ELSE
BEGIN
    PRINT 'Usando sesión existente: ID ' + CAST(@Id_Sesion AS VARCHAR(10));
END

-- Crear factura de prueba
INSERT INTO Factura (
    CodigoFactura,
    Id_Cliente,
    Id_Sesion,
    NumeroFactura,
    Subtotal,
    Descuento,
    Impuesto,
    EstadoFactura,
    Id_UsuarioCreacion,
    FechaCreacion
)
VALUES (
    @CodigoFactura,
    NULL, -- Cliente general
    @Id_Sesion,
    @CodigoFactura,
    156.00, -- Subtotal: (4 * 12) + (9 * 12) = 156
    0.00,
    0.00,
    'ACTIVA',
    @Id_UsuarioCreacion,
    GETDATE()
);

SET @Id_Factura = SCOPE_IDENTITY();
PRINT 'Factura de prueba creada: ID ' + CAST(@Id_Factura AS VARCHAR(10)) + ', Código: ' + @CodigoFactura;
GO

-- Paso 3: Probar el stored procedure con una transacción
-- Esto simula exactamente lo que hace la aplicación
BEGIN TRANSACTION;
BEGIN TRY
    DECLARE @Id_Factura INT;
    DECLARE @Id_UsuarioCreacion INT = 1; -- Ajusta según tu usuario
    DECLARE @Resultado1 INT;
    DECLARE @Resultado2 INT;
    DECLARE @ErrorMsg NVARCHAR(4000);
    
    -- Obtener el ID de la factura de prueba que acabamos de crear
    SELECT TOP 1 @Id_Factura = Id_Factura 
    FROM Factura 
    WHERE CodigoFactura LIKE 'TEST%' 
    ORDER BY FechaCreacion DESC;
    
    IF @Id_Factura IS NULL
    BEGIN
        RAISERROR('No se encontró una factura de prueba. Ejecuta primero la parte de creación de factura.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    PRINT '=== INICIANDO PRUEBA DEL STORED PROCEDURE ===';
    PRINT 'Factura ID: ' + CAST(@Id_Factura AS VARCHAR(10));
    PRINT '';
    
    -- Probar con el lote 210 (4 unidades)
    PRINT '--- Procesando Lote 210 (4 unidades) ---';
    EXEC @Resultado1 = sp_InsertarDetalleFactura
        @Id_Factura = @Id_Factura,
        @Id_Lote = 210,
        @Cantidad = 4,
        @PrecioUnitario = 12.00,
        @Id_UsuarioCreacion = @Id_UsuarioCreacion;
    
    IF @Resultado1 = 1
        PRINT '✓ Lote 210 procesado exitosamente';
    ELSE
    BEGIN
        SET @ErrorMsg = ERROR_MESSAGE();
        PRINT '✗ Error al procesar Lote 210: ' + @ErrorMsg;
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    PRINT '';
    
    -- Probar con el lote 211 (9 unidades)
    PRINT '--- Procesando Lote 211 (9 unidades) ---';
    EXEC @Resultado2 = sp_InsertarDetalleFactura
        @Id_Factura = @Id_Factura,
        @Id_Lote = 211,
        @Cantidad = 9,
        @PrecioUnitario = 12.00,
        @Id_UsuarioCreacion = @Id_UsuarioCreacion;
    
    IF @Resultado2 = 1
        PRINT '✓ Lote 211 procesado exitosamente';
    ELSE
    BEGIN
        SET @ErrorMsg = ERROR_MESSAGE();
        PRINT '✗ Error al procesar Lote 211: ' + @ErrorMsg;
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    PRINT '';
    PRINT '=== PRUEBA EXITOSA ===';
    PRINT 'Ambos lotes fueron procesados correctamente.';
    PRINT '';
    PRINT '¿Deseas hacer COMMIT o ROLLBACK?';
    PRINT 'Para confirmar los cambios: COMMIT TRANSACTION;';
    PRINT 'Para deshacer los cambios: ROLLBACK TRANSACTION;';
    
    -- Por defecto, hacemos ROLLBACK para no afectar los datos reales
    -- Descomenta la siguiente línea si quieres confirmar los cambios:
    -- COMMIT TRANSACTION;
    
    -- Por ahora, hacemos ROLLBACK para que sea una prueba segura
    ROLLBACK TRANSACTION;
    PRINT '';
    PRINT 'ROLLBACK ejecutado - Los cambios fueron revertidos (prueba segura)';
    
END TRY
BEGIN CATCH
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
    
    PRINT '=== ERROR EN LA PRUEBA ===';
    PRINT 'Mensaje: ' + @ErrorMessage;
    PRINT 'Severidad: ' + CAST(@ErrorSeverity AS VARCHAR(10));
    PRINT 'Estado: ' + CAST(@ErrorState AS VARCHAR(10));
    
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;
GO

-- Paso 4: Verificar el stock después de la prueba (si hiciste COMMIT)
-- Si hiciste ROLLBACK, el stock debería ser el mismo que al inicio
PRINT '';
PRINT '=== STOCK DESPUÉS DE LA PRUEBA ===';
SELECT 
    Id_Lote,
    CodigoLote,
    Cantidad AS StockActual,
    Estado,
    FechaVencimiento
FROM Lote
WHERE Id_Lote IN (210, 211)
ORDER BY Id_Lote;
GO

-- Paso 5: Verificar los detalles de factura creados (si hiciste COMMIT)
PRINT '';
PRINT '=== DETALLES DE FACTURA CREADOS ===';
SELECT 
    df.Id_DetalleFactura,
    df.Id_Factura,
    df.Id_Lote,
    l.CodigoLote,
    df.Cantidad,
    df.PrecioUnitario,
    df.FechaCreacion
FROM DetalleFactura df
INNER JOIN Lote l ON df.Id_Lote = l.Id_Lote
WHERE df.Id_Factura = (SELECT TOP 1 Id_Factura FROM Factura WHERE CodigoFactura LIKE 'TEST%' ORDER BY FechaCreacion DESC)
ORDER BY df.Id_Lote;
GO

