-- =============================================
-- Script COMPLETO y SIMPLE para arreglar facturación
-- Versión simplificada que funciona
-- =============================================

USE SuperLaEsperanzaDB;
GO

PRINT '=== Iniciando corrección completa ===';
GO

-- =============================================
-- Paso 1: Asegurar que el constraint permite 0
-- =============================================
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_Lote_Cantidad' AND parent_object_id = OBJECT_ID('Lote'))
BEGIN
    ALTER TABLE Lote DROP CONSTRAINT CHK_Lote_Cantidad;
    PRINT 'Constraint CHK_Lote_Cantidad eliminado.';
END
GO

ALTER TABLE Lote ADD CONSTRAINT CHK_Lote_Cantidad CHECK ([Cantidad] >= 0);
PRINT 'Constraint CHK_Lote_Cantidad recreado (permite >= 0).';
GO

-- =============================================
-- Paso 2: Recrear el Stored Procedure SIMPLE
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarDetalleFactura]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_InsertarDetalleFactura]
GO

CREATE PROCEDURE [dbo].[sp_InsertarDetalleFactura]
    @Id_Factura INT,
    @Id_Lote INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @Id_UsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StockDisponible INT;
    DECLARE @EstadoLote BIT;
    DECLARE @CodigoLote VARCHAR(10);
    DECLARE @NombreProducto VARCHAR(255);
    
    BEGIN TRY
        -- 1. Leer y bloquear el lote
        SELECT 
            @StockDisponible = l.Cantidad,
            @EstadoLote = l.Estado,
            @CodigoLote = l.CodigoLote,
            @NombreProducto = p.Nombre
        FROM Lote l WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
        INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
        WHERE l.Id_Lote = @Id_Lote;
        
        -- 2. Validaciones básicas
        IF @StockDisponible IS NULL
        BEGIN
            RAISERROR('El lote con ID %d no existe.', 16, 1, @Id_Lote);
            RETURN -1;
        END;
        
        IF @EstadoLote = 0
        BEGIN
            RAISERROR('El lote %s está inactivo.', 16, 1, @CodigoLote);
            RETURN -1;
        END;
        
        IF @StockDisponible < @Cantidad
        BEGIN
            RAISERROR('Stock insuficiente para "%s" (Lote: %s). Disponible: %d, Solicitado: %d', 
                      16, 1, @NombreProducto, @CodigoLote, @StockDisponible, @Cantidad);
            RETURN -1;
        END;
        
        IF @Cantidad <= 0
        BEGIN
            RAISERROR('La cantidad debe ser mayor a cero.', 16, 1);
            RETURN -1;
        END;
        
        IF @PrecioUnitario <= 0
        BEGIN
            RAISERROR('El precio unitario debe ser mayor a cero.', 16, 1);
            RETURN -1;
        END;
        
        -- 3. Verificar que no exista ya un detalle para este lote en esta factura
        IF EXISTS (SELECT 1 FROM DetalleFactura WHERE Id_Factura = @Id_Factura AND Id_Lote = @Id_Lote)
        BEGIN
            RAISERROR('Ya existe un detalle para el lote ID %d en la factura ID %d.', 16, 1, @Id_Lote, @Id_Factura);
            RETURN -1;
        END;
        
        -- 4. Actualizar el stock (con validación en WHERE)
        UPDATE Lote
        SET Cantidad = Cantidad - @Cantidad,
            FechaModificacion = GETDATE(),
            Id_UsuarioModificacion = @Id_UsuarioCreacion
        WHERE Id_Lote = @Id_Lote
          AND Cantidad = @StockDisponible  -- Solo si el stock no cambió
          AND Cantidad >= @Cantidad;       -- Solo si hay suficiente stock
        
        -- 5. Verificar que el UPDATE fue exitoso
        IF @@ROWCOUNT = 0
        BEGIN
            SELECT @StockDisponible = Cantidad FROM Lote WHERE Id_Lote = @Id_Lote;
            RAISERROR('Stock insuficiente para "%s" (Lote: %s). Stock actual: %d, Solicitado: %d', 
                      16, 1, @NombreProducto, @CodigoLote, @StockDisponible, @Cantidad);
            RETURN -1;
        END;
        
        -- 6. Si el stock llegó a 0, marcar como inactivo
        IF (SELECT Cantidad FROM Lote WHERE Id_Lote = @Id_Lote) = 0
        BEGIN
            UPDATE Lote
            SET Estado = 0,
                FechaModificacion = GETDATE(),
                Id_UsuarioModificacion = @Id_UsuarioCreacion
            WHERE Id_Lote = @Id_Lote;
        END;
        
        -- 7. Insertar el detalle
        INSERT INTO DetalleFactura (
            Id_Factura,
            Id_Lote,
            Cantidad,
            PrecioUnitario,
            Id_UsuarioCreacion,
            FechaCreacion
        )
        VALUES (
            @Id_Factura,
            @Id_Lote,
            @Cantidad,
            @PrecioUnitario,
            @Id_UsuarioCreacion,
            GETDATE()
        );
        
        RETURN 1;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN -1;
    END CATCH;
END;
GO

GRANT EXECUTE ON [dbo].[sp_InsertarDetalleFactura] TO [public];
GO

PRINT '=== Stored Procedure sp_InsertarDetalleFactura creado correctamente ===';
PRINT '=== Script completado exitosamente ===';
GO

