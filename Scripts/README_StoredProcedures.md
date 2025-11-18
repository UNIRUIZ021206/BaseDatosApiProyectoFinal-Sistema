# Scripts de Stored Procedures Mejorados

## sp_InsertarDetalleFactura_Mejorado.sql

### Descripción
Este stored procedure ha sido mejorado para manejar correctamente la validación y actualización del stock con locks apropiados para evitar problemas de concurrencia.

### Mejoras implementadas:

1. **UPDLOCK y ROWLOCK**: Bloquea el registro del lote durante la validación e inserción para evitar condiciones de carrera
2. **Validaciones completas**: 
   - Verifica que el lote existe
   - Verifica que el lote está activo
   - Verifica que hay stock suficiente
   - Valida que la cantidad y precio sean positivos
   - Verifica que no exista ya un detalle con el mismo Id_Factura e Id_Lote (constraint UNIQUE)
3. **Mensajes de error descriptivos**: Incluye información del producto, lote, stock disponible y solicitado
4. **Transacciones seguras**: Usa SET XACT_ABORT ON para rollback automático en caso de error
5. **Doble verificación**: Verifica que el stock no quede negativo después de la actualización
6. **Compatibilidad con estructura de BD**: 
   - No intenta insertar el campo `Subtotal` (no existe en la tabla)
   - Respeta el constraint `UQ_DetalleFactura UNIQUE(Id_Factura, Id_Lote)`

### Instrucciones de instalación:

1. Conectarse a la base de datos SQL Server
2. Ejecutar el script `sp_InsertarDetalleFactura_Mejorado.sql`
3. Verificar que el procedimiento se creó correctamente:
   ```sql
   SELECT * FROM sys.procedures WHERE name = 'sp_InsertarDetalleFactura'
   ```

### Notas importantes:

- El procedimiento reemplaza el procedimiento existente si ya existe
- Todos los permisos se otorgan automáticamente
- El procedimiento devuelve 1 si es exitoso, -1 si hay error
- Los errores se lanzan con RAISERROR para que la API pueda capturarlos
- **IMPORTANTE**: El código C# ya agrupa los detalles por lote antes de llamar al SP, lo cual es necesario debido al constraint `UQ_DetalleFactura UNIQUE(Id_Factura, Id_Lote)`
- La tabla `DetalleFactura` NO tiene campo `Subtotal`, solo `Cantidad` y `PrecioUnitario` (el subtotal se calcula cuando sea necesario)

### Uso desde la API:

El código C# ya está actualizado para usar este procedimiento. No se requieren cambios adicionales en la aplicación.

