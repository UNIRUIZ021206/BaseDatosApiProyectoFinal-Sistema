-- =============================================
-- Stored Procedure: sp_InsertarDetalleFactura
-- Descripción: Inserta un detalle de factura y actualiza el stock del lote
--              con validación y bloqueo para evitar problemas de concurrencia
-- =============================================

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
    SET XACT_ABORT ON; -- Rollback automático en caso de error
    
    DECLARE @StockDisponible INT;
    DECLARE @EstadoLote BIT;
    DECLARE @CodigoLote VARCHAR(10);
    DECLARE @NombreProducto VARCHAR(255);
    DECLARE @PrecioStr VARCHAR(20);
    DECLARE @StockActual INT;
    DECLARE @StockVerificacion INT;
    DECLARE @StockFinal INT;
    DECLARE @Id_Lote_Verificado INT;
    DECLARE @Id_Lote_Verificado_Str VARCHAR(10);
    DECLARE @Resultado INT = 0;
    
    BEGIN TRY
        -- NO iniciar una nueva transacción aquí porque ya estamos dentro de una transacción
        -- que fue iniciada en el DAO. Solo validar y actualizar.
        
        -- Validar y bloquear el lote con UPDLOCK para evitar problemas de concurrencia
        -- Leer toda la información necesaria en una sola consulta
        -- UPDLOCK + HOLDLOCK mantiene el lock hasta el final de la transacción
        -- UPDLOCK ya es un lock exclusivo para actualización, no necesita XLOCK
        -- IMPORTANTE: Asegurar que estamos procesando el lote correcto usando @Id_Lote
        -- Usar una variable temporal para verificar que encontramos el lote correcto
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
        -- Esto asegura que estamos procesando el lote correcto
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
        -- (debido al constraint UQ_DetalleFactura UNIQUE(Id_Factura, Id_Lote))
        IF EXISTS (SELECT 1 FROM DetalleFactura WHERE Id_Factura = @Id_Factura AND Id_Lote = @Id_Lote)
        BEGIN
            RAISERROR('Ya existe un detalle de factura para el lote ID %d en la factura ID %d. Use UPDATE en lugar de INSERT.', 
                      16, 1, @Id_Lote, @Id_Factura);
            RETURN -1;
        END;
        
        -- Actualizar el stock PRIMERO mientras el lock está activo
        -- Esto asegura que el stock no cambie antes de insertar el detalle
        -- El UPDLOCK con HOLDLOCK de la lectura inicial mantiene el lock durante toda la transacción
        -- IMPORTANTE: Usar una subconsulta en el UPDATE para asegurar que el stock no cambió
        -- y que el resultado no violará el constraint CHK_Lote_Cantidad
        UPDATE l
        SET l.Cantidad = l.Cantidad - @Cantidad,
            l.FechaModificacion = GETDATE(),
            l.Id_UsuarioModificacion = @Id_UsuarioCreacion
        FROM Lote l WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
        WHERE l.Id_Lote = @Id_Lote
          AND l.Cantidad = @StockDisponible  -- Usar el stock exacto que leímos para asegurar que no cambió
          AND l.Estado = 1                    -- Asegurar que el lote está activo
          AND l.Cantidad >= @Cantidad;       -- Validación adicional: asegurar que tenemos stock suficiente
        
        -- Verificar que la actualización del stock fue exitosa ANTES de insertar el detalle
        IF @@ROWCOUNT = 0
        BEGIN
            -- Si no se actualizó, el stock cambió entre la lectura y el UPDATE
            -- Leer el stock actual para el mensaje de error (sin lock porque ya sabemos que falló)
            SELECT @StockActual = Cantidad FROM Lote WHERE Id_Lote = @Id_Lote;
            
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
        
        -- Verificar que el stock no quedó negativo después del UPDATE (esto no debería pasar)
        SELECT @StockFinal = Cantidad FROM Lote WHERE Id_Lote = @Id_Lote;
        IF @StockFinal < 0
        BEGIN
            RAISERROR('Error: El stock del lote ID %d (Código: %s) quedó negativo después de la actualización. Stock final: %d', 
                      16, 1, @Id_Lote, @CodigoLote, @StockFinal);
            RETURN -1;
        END;
        
        -- Insertar el detalle de factura DESPUÉS de actualizar el stock
        -- Nota: Subtotal no existe en la tabla, se calcula como Cantidad * PrecioUnitario cuando sea necesario
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
        
        -- NO hacer COMMIT aquí porque la transacción es manejada por el DAO
        SET @Resultado = 1;
        
    END TRY
    BEGIN CATCH
        -- NO hacer ROLLBACK aquí porque la transacción es manejada por el DAO
        -- Solo re-lanzar el error con información adicional
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN -1;
    END CATCH;
    
    RETURN @Resultado;
END;
GO

-- Otorgar permisos de ejecución
GRANT EXECUTE ON [dbo].[sp_InsertarDetalleFactura] TO [public];
GO

