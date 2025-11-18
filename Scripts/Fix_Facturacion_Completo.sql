-- =============================================
-- Script de Corrección Completo para Facturación
-- Soluciona el error de constraint CHK_Lote_Cantidad y mejora el stored procedure
-- =============================================

USE SuperLaEsperanzaDB;
GO

-- =============================================
-- Paso 1: Verificar y modificar el constraint CHK_Lote_Cantidad
-- =============================================
PRINT '=== Modificando constraint CHK_Lote_Cantidad ===';

-- Verificar si el constraint existe y eliminarlo si es necesario
IF EXISTS (
    SELECT 1 
    FROM sys.check_constraints 
    WHERE name = 'CHK_Lote_Cantidad' 
    AND parent_object_id = OBJECT_ID('Lote')
)
BEGIN
    ALTER TABLE Lote
    DROP CONSTRAINT CHK_Lote_Cantidad;
    PRINT 'Constraint CHK_Lote_Cantidad eliminado.';
END
GO

-- Crear el constraint corregido que permite 0 (lotes agotados)
ALTER TABLE Lote
ADD CONSTRAINT CHK_Lote_Cantidad CHECK ([Cantidad] >= 0);
GO

PRINT 'Constraint CHK_Lote_Cantidad recreado correctamente (permite Cantidad >= 0).';
GO

-- =============================================
-- Paso 2: Recrear el Stored Procedure sp_InsertarDetalleFactura
-- =============================================
PRINT '';
PRINT '=== Recreando Stored Procedure sp_InsertarDetalleFactura ===';

-- Si el procedimiento ya existe, eliminarlo primero
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
    -- NO usar SET XACT_ABORT ON para permitir manejo de errores controlado
    
    DECLARE @StockDisponible INT;
    DECLARE @EstadoLote BIT;
    DECLARE @CodigoLote VARCHAR(10);
    DECLARE @NombreProducto VARCHAR(255);
    DECLARE @PrecioStr VARCHAR(20);
    DECLARE @StockActual INT;
    DECLARE @StockFinal INT;
    DECLARE @StockFinalCalculado INT;
    DECLARE @Id_Lote_Verificado INT;
    DECLARE @Id_Lote_Verificado_Str VARCHAR(10);
    DECLARE @Resultado INT = 0;
    
    BEGIN TRY
        -- NO iniciar una nueva transacción aquí porque ya estamos dentro de una transacción
        -- que fue iniciada en el DAO. Solo validar y actualizar.
        
        -- Validar y bloquear el lote con UPDLOCK para evitar problemas de concurrencia
        -- Leer toda la información necesaria en una sola consulta
        -- UPDLOCK + ROWLOCK + HOLDLOCK mantiene el lock hasta el final de la transacción
        SELECT 
            @Id_Lote_Verificado = l.Id_Lote,
            @StockDisponible = l.Cantidad,
            @EstadoLote = l.Estado,
            @CodigoLote = l.CodigoLote,
            @NombreProducto = p.Nombre
        FROM Lote l WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
        INNER JOIN Producto p ON l.Id_Producto = p.Id_Producto
        WHERE l.Id_Lote = @Id_Lote;
        
        -- Verificar que encontramos exactamente el lote que buscamos
        IF @Id_Lote_Verificado IS NULL OR @Id_Lote_Verificado != @Id_Lote
        BEGIN
            SET @Id_Lote_Verificado_Str = CASE 
                WHEN @Id_Lote_Verificado IS NULL THEN 'NULL' 
                ELSE CAST(@Id_Lote_Verificado AS VARCHAR(10)) 
            END;
            RAISERROR('Error crítico: El lote con ID %d no existe o no se pudo encontrar. Lote verificado: %s', 
                      16, 1, @Id_Lote, @Id_Lote_Verificado_Str);
            RETURN -1;
        END;
        
        -- Verificar que el stock se leyó correctamente
        IF @StockDisponible IS NULL
        BEGIN
            RAISERROR('Error crítico: No se pudo leer el stock del lote con ID %d. El lote existe pero el stock es NULL.', 16, 1, @Id_Lote);
            RETURN -1;
        END;
        
        -- Verificar que el lote recibido coincide con el que encontramos
        IF @CodigoLote IS NULL OR @CodigoLote = ''
        BEGIN
            RAISERROR('Error: No se pudo obtener el código del lote ID %d.', 16, 1, @Id_Lote);
            RETURN -1;
        END;
        
        -- Verificar que el lote está activo
        IF @EstadoLote = 0
        BEGIN
            RAISERROR('El lote %s está inactivo o vencido.', 16, 1, @CodigoLote);
            RETURN -1;
        END;
        
        -- Verificar que hay stock suficiente
        IF @StockDisponible < @Cantidad
        BEGIN
            RAISERROR('Stock insuficiente para el producto "%s" (Lote ID: %d, Código: %s). Stock disponible: %d, solicitado: %d', 
                      16, 1, @NombreProducto, @Id_Lote, @CodigoLote, @StockDisponible, @Cantidad);
            RETURN -1;
        END;
        
        -- Validar que la cantidad sea positiva
        IF @Cantidad <= 0
        BEGIN
            RAISERROR('La cantidad debe ser mayor a cero. Cantidad recibida: %d', 16, 1, @Cantidad);
            RETURN -1;
        END;
        
        -- Validar que el precio sea positivo
        IF @PrecioUnitario <= 0
        BEGIN
            SET @PrecioStr = CAST(@PrecioUnitario AS VARCHAR(20));
            RAISERROR('El precio unitario debe ser mayor a cero. Precio recibido: %s', 16, 1, @PrecioStr);
            RETURN -1;
        END;
        
        -- Verificar que no exista ya un detalle con el mismo Id_Factura e Id_Lote
        IF EXISTS (SELECT 1 FROM DetalleFactura WHERE Id_Factura = @Id_Factura AND Id_Lote = @Id_Lote)
        BEGIN
            RAISERROR('Ya existe un detalle de factura para el lote ID %d en la factura ID %d. Use UPDATE en lugar de INSERT.', 
                      16, 1, @Id_Lote, @Id_Factura);
            RETURN -1;
        END;
        
        -- Calcular el stock final ANTES de actualizar para validar que no será negativo
        -- Esto es una validación adicional de seguridad
        SET @StockFinalCalculado = @StockDisponible - @Cantidad;
        
        IF @StockFinalCalculado < 0
        BEGIN
            RAISERROR('Error: El cálculo del stock final resultaría en un valor negativo (%d - %d = %d). Esto no debería ocurrir después de las validaciones previas.', 
                      16, 1, @StockDisponible, @Cantidad, @StockFinalCalculado);
            RETURN -1;
        END;
        
        -- Actualizar el stock PRIMERO mientras el lock está activo
        -- IMPORTANTE: El lock UPDLOCK + HOLDLOCK del SELECT inicial se mantiene durante toda la transacción
        -- Usar UPDATE con FROM para mantener el mismo lock y asegurar consistencia
        -- La cláusula WHERE es CRÍTICA: debe prevenir que el UPDATE se ejecute si:
        -- 1. El stock cambió (Cantidad != @StockDisponible)
        -- 2. El resultado sería negativo (Cantidad - @Cantidad < 0)
        -- Esto previene físicamente que SQL intente violar el constraint CHK_Lote_Cantidad
        UPDATE l
        SET l.Cantidad = @StockFinalCalculado,  -- Usar el valor calculado en lugar de restar directamente
            l.FechaModificacion = GETDATE(),
            l.Id_UsuarioModificacion = @Id_UsuarioCreacion
        FROM Lote l WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
        WHERE l.Id_Lote = @Id_Lote
          AND l.Cantidad = @StockDisponible  -- CRÍTICO: Solo actualizar si el stock NO cambió desde la lectura
          AND l.Estado = 1                   -- Asegurar que el lote está activo
          AND l.Cantidad >= @Cantidad;       -- CRÍTICO: Validar que hay suficiente stock ANTES de restar
        
        -- Verificar que la actualización del stock fue exitosa
        IF @@ROWCOUNT = 0
        BEGIN
            -- Si no se actualizó, el stock cambió entre la lectura y el UPDATE o no hay suficiente stock
            -- Leer el stock actual SIN lock porque ya sabemos que falló
            SELECT @StockActual = Cantidad FROM Lote WHERE Id_Lote = @Id_Lote;
            
            IF @StockActual IS NULL
            BEGIN
                RAISERROR('Error crítico: El lote ID %d no existe o fue eliminado durante la operación.', 16, 1, @Id_Lote);
                RETURN -1;
            END
            
            IF @StockActual < @Cantidad
            BEGIN
                RAISERROR('Stock insuficiente para el producto "%s" (Lote ID: %d, Código: %s). Stock disponible: %d, solicitado: %d. El stock cambió durante la operación (era %d al inicio).', 
                          16, 1, @NombreProducto, @Id_Lote, @CodigoLote, @StockActual, @Cantidad, @StockDisponible);
            END
            ELSE
            BEGIN
                RAISERROR('Error al actualizar el stock del lote ID %d (Código: %s). El stock cambió de %d a %d durante la operación. Esto no debería ocurrir con UPDLOCK + HOLDLOCK.', 
                          16, 1, @Id_Lote, @CodigoLote, @StockDisponible, @StockActual);
            END;
            RETURN -1;
        END;
        
        -- Verificar el stock final después del UPDATE (sin lock porque ya está actualizado)
        SELECT @StockFinal = Cantidad FROM Lote WHERE Id_Lote = @Id_Lote;
        
        -- Si el stock llegó a 0, marcar el lote como inactivo
        IF @StockFinal = 0
        BEGIN
            UPDATE Lote
            SET Estado = 0,  -- Marcar como inactivo (agotado)
                FechaModificacion = GETDATE(),
                Id_UsuarioModificacion = @Id_UsuarioCreacion
            WHERE Id_Lote = @Id_Lote;
        END;
        
        -- Insertar el detalle de factura DESPUÉS de actualizar el stock
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
        
        -- Verificar que el INSERT fue exitoso
        IF @@ERROR <> 0
        BEGIN
            RAISERROR('Error al insertar el detalle de factura para el lote ID %d.', 16, 1, @Id_Lote);
            RETURN -1;
        END
        
        -- NO hacer COMMIT aquí porque la transacción es manejada por el DAO
        SET @Resultado = 1;
        
    END TRY
    BEGIN CATCH
        -- NO hacer ROLLBACK aquí porque la transacción es manejada por el DAO
        -- Solo re-lanzar el error con información adicional
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        
        -- Si es un error de constraint, proporcionar un mensaje más claro
        IF @ErrorNumber = 547  -- Constraint violation
        BEGIN
            SET @ErrorMessage = 'Error de restricción: ' + @ErrorMessage + 
                               ' (Lote ID: ' + CAST(@Id_Lote AS VARCHAR(10)) + 
                               ', Stock disponible: ' + CAST(ISNULL(@StockDisponible, 0) AS VARCHAR(10)) + 
                               ', Cantidad solicitada: ' + CAST(@Cantidad AS VARCHAR(10)) + ')';
        END
        
        -- Asegurar que siempre se retorne un valor, incluso si hay un error
        SET @Resultado = -1;
        
        -- Lanzar el error con severidad suficiente para que sea capturado como excepción
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        
        -- Retornar el código de error
        RETURN @Resultado;
    END CATCH;
    
    -- Asegurar que siempre se retorne un valor
    IF @Resultado IS NULL
    BEGIN
        SET @Resultado = -1;
    END
    
    RETURN @Resultado;
END;
GO

-- Otorgar permisos de ejecución
GRANT EXECUTE ON [dbo].[sp_InsertarDetalleFactura] TO [public];
GO

PRINT '';
PRINT '=== Stored Procedure sp_InsertarDetalleFactura recreado correctamente ===';
PRINT '';
PRINT 'Script de corrección completado exitosamente.';
PRINT 'Por favor, verifica que el constraint y el stored procedure se crearon correctamente.';
GO

