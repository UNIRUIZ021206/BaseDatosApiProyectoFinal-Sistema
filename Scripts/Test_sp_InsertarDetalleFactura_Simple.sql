-- =============================================
-- Script de Prueba SIMPLE para sp_InsertarDetalleFactura
-- Prueba directa del stored procedure con una factura existente
-- =============================================

-- IMPORTANTE: Ajusta estos valores según tu base de datos
DECLARE @Id_Factura INT = 1; -- Cambia por un ID de factura existente o crea una nueva
DECLARE @Id_UsuarioCreacion INT = 1; -- Cambia por tu ID de usuario

-- Verificar stock antes
PRINT '=== STOCK ANTES ===';
SELECT Id_Lote, CodigoLote, Cantidad, Estado 
FROM Lote 
WHERE Id_Lote IN (210, 211)
ORDER BY Id_Lote;
GO

-- Si necesitas crear una factura de prueba primero, ejecuta esto:
/*
DECLARE @Id_Factura INT;
DECLARE @Id_Sesion INT;
DECLARE @Id_UsuarioCreacion INT = 1;
DECLARE @Id_Usuario INT = 1;
DECLARE @CodigoFactura VARCHAR(10) = 'TEST' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(6));
DECLARE @CodigoSesion VARCHAR(10) = 'TEST' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(6));

-- Buscar sesión activa (Estado = 1 = Abierta)
SELECT TOP 1 @Id_Sesion = Id_Sesion FROM Sesion WHERE Estado = 1 ORDER BY FechaApertura DESC;

-- Si no hay sesión activa, crear una
IF @Id_Sesion IS NULL
BEGIN
    INSERT INTO Sesion (CodigoSesion, Id_Usuario, FechaApertura, MontoInicial, Estado, Id_UsuarioCreacion, FechaCreacion)
    VALUES (@CodigoSesion, @Id_Usuario, GETDATE(), 0.00, 1, @Id_UsuarioCreacion, GETDATE());
    SET @Id_Sesion = SCOPE_IDENTITY();
END

INSERT INTO Factura (CodigoFactura, Id_Sesion, NumeroFactura, Subtotal, Descuento, Impuesto, EstadoFactura, Id_UsuarioCreacion, FechaCreacion)
VALUES (@CodigoFactura, @Id_Sesion, @CodigoFactura, 156.00, 0.00, 0.00, 'ACTIVA', @Id_UsuarioCreacion, GETDATE());

SET @Id_Factura = SCOPE_IDENTITY();
PRINT 'Factura creada: ID ' + CAST(@Id_Factura AS VARCHAR(10));
*/

-- =============================================
-- PRUEBA DEL STORED PROCEDURE
-- =============================================
BEGIN TRANSACTION;
BEGIN TRY
    DECLARE @Id_Factura INT = 1; -- CAMBIA ESTE VALOR
    DECLARE @Id_UsuarioCreacion INT = 1; -- CAMBIA ESTE VALOR
    DECLARE @Resultado INT;
    
    PRINT '=== PROBANDO LOTE 210 (4 unidades) ===';
    EXEC @Resultado = sp_InsertarDetalleFactura
        @Id_Factura = @Id_Factura,
        @Id_Lote = 210,
        @Cantidad = 4,
        @PrecioUnitario = 12.00,
        @Id_UsuarioCreacion = @Id_UsuarioCreacion;
    
    IF @Resultado = 1
        PRINT '✓ Lote 210: EXITOSO';
    ELSE
    BEGIN
        PRINT '✗ Lote 210: FALLÓ (Código: ' + CAST(@Resultado AS VARCHAR(10)) + ')';
        PRINT 'Error: ' + ERROR_MESSAGE();
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    PRINT '';
    PRINT '=== PROBANDO LOTE 211 (9 unidades) ===';
    EXEC @Resultado = sp_InsertarDetalleFactura
        @Id_Factura = @Id_Factura,
        @Id_Lote = 211,
        @Cantidad = 9,
        @PrecioUnitario = 12.00,
        @Id_UsuarioCreacion = @Id_UsuarioCreacion;
    
    IF @Resultado = 1
        PRINT '✓ Lote 211: EXITOSO';
    ELSE
    BEGIN
        PRINT '✗ Lote 211: FALLÓ (Código: ' + CAST(@Resultado AS VARCHAR(10)) + ')';
        PRINT 'Error: ' + ERROR_MESSAGE();
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    PRINT '';
    PRINT '=== AMBOS LOTES PROCESADOS EXITOSAMENTE ===';
    
    -- Verificar stock después
    PRINT '';
    PRINT '=== STOCK DESPUÉS ===';
    SELECT Id_Lote, CodigoLote, Cantidad, Estado 
    FROM Lote 
    WHERE Id_Lote IN (210, 211)
    ORDER BY Id_Lote;
    
    -- Descomenta la siguiente línea para confirmar los cambios:
    -- COMMIT TRANSACTION;
    
    -- Por defecto, hacemos ROLLBACK para prueba segura
    ROLLBACK TRANSACTION;
    PRINT '';
    PRINT 'ROLLBACK ejecutado - Cambios revertidos (prueba segura)';
    
END TRY
BEGIN CATCH
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorNumber INT = ERROR_NUMBER();
    
    PRINT '=== ERROR ===';
    PRINT 'Número: ' + CAST(@ErrorNumber AS VARCHAR(10));
    PRINT 'Mensaje: ' + @ErrorMessage;
    
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;
GO

