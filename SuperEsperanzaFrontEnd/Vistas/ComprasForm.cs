using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class ComprasForm : Form
    {
        private readonly CompraRepository _compraRepo;
        private readonly ProveedorRepository _proveedorRepo;
        private readonly ProductoRepository _productoRepo;
        private List<ProveedorDto> _proveedores = new();
        private List<ProductoDto> _productos = new();
        private List<DetalleCompraItem> _detalles = new();

        private class DetalleCompraItem
        {
            public int Id_Producto { get; set; }
            public string NombreProducto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal => Cantidad * PrecioUnitario;
        }

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public ComprasForm()
        {
            InitializeComponent();
            _compraRepo = new CompraRepository();
            _proveedorRepo = new ProveedorRepository();
            _productoRepo = new ProductoRepository();
            
            // Configurar el DateTimePicker para mostrar solo la fecha
            dtpFechaCompra.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFechaCompra.ShowUpDown = false;
            // Establecer la fecha actual, asegurándose de que esté en el rango válido
            // DateTimePicker admite fechas entre 1753-01-01 y 9998-12-31
            var fechaActual = DateTime.Now.Date;
            if (fechaActual < new DateTime(1753, 1, 1))
                dtpFechaCompra.Value = new DateTime(1753, 1, 1);
            else if (fechaActual > new DateTime(9998, 12, 31))
                dtpFechaCompra.Value = new DateTime(9998, 12, 31);
            else
                dtpFechaCompra.Value = fechaActual;
            
            this.Shown += async (s, e) => await CargarDatos();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvDetalles);
        }

        private string GenerarCodigoCompraUnico()
        {
            var random = new Random();
            var now = DateTime.Now;
            
            // Formato de 10 caracteres: COM + DDMM + HH + R
            // COM (3) + DDMM (4) + HH (2) + R (1) = 10 caracteres exactos
            // DDMM = día y mes (2 dígitos cada uno)
            // HH = hora (2 dígitos)
            // R = 1 dígito aleatorio (0-9) para evitar colisiones en la misma hora
            
            var diaMes = $"{now.Day:00}{now.Month:00}"; // DDMM (4 caracteres)
            var hora = $"{now.Hour:00}"; // HH (2 caracteres)
            var aleatorio = random.Next(0, 10); // 1 dígito aleatorio (0-9)
            
            // Formato final: COM + DDMM + HH + R = 10 caracteres
            var codigo = $"COM{diaMes}{hora}{aleatorio}";
            
            return codigo;
        }

        private async Task CargarDatos()
        {
            try
            {
                lblEstado.Text = "Cargando datos...";
                HabilitarControles(false);

                var tareaProveedores = _proveedorRepo.GetAllAsync();
                var tareaProductos = _productoRepo.GetAllAsync();

                await Task.WhenAll(tareaProveedores, tareaProductos);

                _proveedores = await tareaProveedores;
                _productos = await tareaProductos;

                cmbProveedor.DataSource = _proveedores.Where(p => p.Estado).OrderBy(p => p.Nombre).ToList();
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "Id_Proveedor";

                cmbProducto.DataSource = _productos.Where(p => p.Estado).OrderBy(p => p.Nombre).ToList();
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "Id_Producto";

                ActualizarGridDetalles();
                ActualizarTotal();

                lblEstado.Text = "Listo";
                HabilitarControles(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGridDetalles()
        {
            dgvDetalles.Rows.Clear();
            foreach (var detalle in _detalles)
            {
                dgvDetalles.Rows.Add(
                    detalle.Id_Producto,
                    detalle.NombreProducto,
                    detalle.Cantidad,
                    detalle.PrecioUnitario.ToString("C"),
                    detalle.Subtotal.ToString("C")
                );
            }
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            var total = _detalles.Sum(d => d.Subtotal);
            lblTotal.Text = total.ToString("C");
        }

        private void HabilitarControles(bool habilitar)
        {
            cmbProveedor.Enabled = habilitar;
            dtpFechaCompra.Enabled = habilitar;
            cmbProducto.Enabled = habilitar;
            numCantidad.Enabled = habilitar;
            numPrecioUnitario.Enabled = habilitar;
            btnAgregar.Enabled = habilitar;
            btnEliminar.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            dgvDetalles.Enabled = habilitar;
        }

        private void btnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProducto.Focus();
                return;
            }

            var producto = cmbProducto.SelectedItem as ProductoDto;
            if (producto == null)
            {
                MessageBox.Show("Error al obtener el producto seleccionado.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var idProducto = producto.Id_Producto;
            var cantidad = (int)numCantidad.Value;
            var precio = numPrecioUnitario.Value;

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numCantidad.Focus();
                numCantidad.Select(0, numCantidad.Text.Length);
                return;
            }

            if (precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a cero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrecioUnitario.Focus();
                numPrecioUnitario.Select(0, numPrecioUnitario.Text.Length);
                return;
            }

            // Verificar si el producto ya existe en los detalles
            var detalleExistente = _detalles.FirstOrDefault(d => d.Id_Producto == idProducto);
            if (detalleExistente != null)
            {
                // Si ya existe, preguntar si desea actualizar o agregar como nuevo
                var respuesta = MessageBox.Show(
                    $"El producto '{producto.Nombre}' ya está en la lista de detalles.\n\n" +
                    "¿Desea actualizar la cantidad y precio?\n\n" +
                    "Sí = Actualizar\nNo = Agregar como nuevo detalle\nCancelar = No hacer nada",
                    "Producto Duplicado",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    // Actualizar el detalle existente
                    detalleExistente.Cantidad = cantidad;
                    detalleExistente.PrecioUnitario = precio;
                    ActualizarGridDetalles();
                    
                    // Limpiar y enfocar
                    numCantidad.Value = 1;
                    numPrecioUnitario.Value = producto.PrecioVenta;
                    cmbProducto.Focus();
                    lblEstado.Text = $"Detalle actualizado: {producto.Nombre}";
                    return;
                }
                else if (respuesta == DialogResult.Cancel)
                {
                    return;
                }
                // Si es No, continúa y agrega como nuevo detalle
            }

            // Agregar nuevo detalle
            var detalle = new DetalleCompraItem
            {
                Id_Producto = idProducto,
                NombreProducto = producto.Nombre ?? "Producto sin nombre",
                Cantidad = cantidad,
                PrecioUnitario = precio
            };

            _detalles.Add(detalle);
            ActualizarGridDetalles();

            // Limpiar y preparar para siguiente entrada
            numCantidad.Value = 1;
            numPrecioUnitario.Value = producto.PrecioVenta;
            cmbProducto.Focus();
            lblEstado.Text = $"Producto agregado: {producto.Nombre} x{cantidad}";
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvDetalles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto de la lista para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filaSeleccionada = dgvDetalles.SelectedRows[0];
            var idProducto = (int)filaSeleccionada.Cells[0].Value;
            var nombreProducto = filaSeleccionada.Cells[1].Value?.ToString() ?? "Producto";

            var confirmacion = MessageBox.Show(
                $"¿Está seguro de eliminar '{nombreProducto}' de la lista de detalles?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                var eliminado = _detalles.RemoveAll(d => d.Id_Producto == idProducto);
                if (eliminado > 0)
                {
                    ActualizarGridDetalles();
                    lblEstado.Text = $"Producto eliminado: {nombreProducto}";
                }
            }
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            if (cmbProveedor.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un proveedor.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto a la compra.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener proveedor y validar fecha antes del try para que estén disponibles en el catch
            var proveedor = cmbProveedor.SelectedItem as ProveedorDto;
            if (proveedor == null)
            {
                MessageBox.Show("Error al obtener el proveedor seleccionado.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Asegurar que la fecha esté en un formato válido para SQL Server
            // SQL Server acepta fechas entre 1/1/1753 y 12/31/9999
            // DateTimePicker admite hasta 12/31/9998
            var fechaSeleccionada = dtpFechaCompra.Value;
            var minDate = new DateTime(1753, 1, 1);
            var maxDate = new DateTime(9998, 12, 31); // Límite del DateTimePicker
            
            // Validar que la fecha esté dentro del rango válido
            if (fechaSeleccionada < minDate || fechaSeleccionada > maxDate)
            {
                MessageBox.Show($"La fecha debe estar entre {minDate:dd/MM/yyyy} y {maxDate:dd/MM/yyyy}.",
                    "Fecha Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asegurar que la fecha esté en formato correcto para SQL Server
            // Extraer solo la parte de fecha (sin hora) y crear una nueva fecha sin zona horaria
            var fechaSoloFecha = fechaSeleccionada.Date;
            
            // Crear una fecha completamente nueva sin zona horaria para evitar problemas de serialización
            var fechaParaEnviar = new DateTime(
                fechaSoloFecha.Year,
                fechaSoloFecha.Month,
                fechaSoloFecha.Day,
                0, 0, 0,
                DateTimeKind.Unspecified
            );
            
            // Validación final: asegurar que la fecha esté en el rango válido de SQL Server
            var sqlMinDate = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var sqlMaxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified);
            
            if (fechaParaEnviar < sqlMinDate || fechaParaEnviar > sqlMaxDate)
            {
                MessageBox.Show($"La fecha generada está fuera del rango válido de SQL Server.\n\nFecha: {fechaParaEnviar:yyyy-MM-dd HH:mm:ss}\nRango válido: {sqlMinDate:yyyy-MM-dd} - {sqlMaxDate:yyyy-MM-dd}",
                    "Error de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblEstado.Text = "Guardando compra...";
                HabilitarControles(false);

                // Generar código único para la compra
                var codigoCompra = GenerarCodigoCompraUnico();

                // Debug: Verificar la fecha antes de enviar
                System.Diagnostics.Debug.WriteLine($"Fecha antes de enviar: {fechaParaEnviar:yyyy-MM-dd HH:mm:ss}, Kind: {fechaParaEnviar.Kind}");
                
                var compra = new CompraCreateDto
                {
                    CodigoCompra = codigoCompra,
                    Id_Proveedor = proveedor.Id_Proveedor,
                    FechaCompra = fechaParaEnviar,
                    Detalles = _detalles.Select(d => new DetalleCompraCreateDto
                    {
                        Id_Producto = d.Id_Producto,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                };

                lblEstado.Text = "Guardando compra y creando lotes...";
                Application.DoEvents();

                var compraCreada = await _compraRepo.CreateAsync(compra);
                if (compraCreada != null)
                {
                    // Recargar productos para actualizar el stock
                    lblEstado.Text = "Actualizando información de productos...";
                    Application.DoEvents();
                    
                    try
                    {
                        _productos = await _productoRepo.GetAllAsync();
                        
                        // Actualizar el combo de productos
                        var productoSeleccionado = cmbProducto.SelectedItem as ProductoDto;
                        cmbProducto.DataSource = _productos.Where(p => p.Estado).OrderBy(p => p.Nombre).ToList();
                        cmbProducto.DisplayMember = "Nombre";
                        cmbProducto.ValueMember = "Id_Producto";
                        
                        // Restaurar selección si es posible
                        if (productoSeleccionado != null)
                        {
                            var nuevoProducto = _productos.FirstOrDefault(p => p.Id_Producto == productoSeleccionado.Id_Producto);
                            if (nuevoProducto != null && nuevoProducto.Estado)
                            {
                                cmbProducto.SelectedValue = nuevoProducto.Id_Producto;
                                if (cmbProducto.SelectedItem != null)
                                {
                                    numPrecioUnitario.Value = nuevoProducto.PrecioVenta;
                                }
                            }
                        }
                    }
                    catch (Exception exRecargar)
                    {
                        // Si falla la recarga, no es crítico, solo lo registramos
                        System.Diagnostics.Debug.WriteLine($"Error al recargar productos: {exRecargar.Message}");
                    }

                    MessageBox.Show(
                        $"Compra creada exitosamente.\n\n" +
                        $"Código: {compraCreada.CodigoCompra}\n" +
                        $"Proveedor: {proveedor.Nombre}\n" +
                        $"Fecha: {compraCreada.FechaCompra:dd/MM/yyyy}\n" +
                        $"Total: {lblTotal.Text}\n\n" +
                        $"El stock de los productos ha sido actualizado automáticamente.",
                        "Compra Exitosa", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    // Limpiar formulario
                    _detalles.Clear();
                    ActualizarGridDetalles();
                    numCantidad.Value = 1;
                    
                    if (cmbProducto.Items.Count > 0)
                    {
                        cmbProducto.SelectedIndex = 0;
                    }
                    
                    lblEstado.Text = "Compra guardada exitosamente";
                }
                else
                {
                    MessageBox.Show("No se pudo crear la compra. Por favor, intente nuevamente.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblEstado.Text = "Error al guardar";
                }
            }
            catch (Exception ex)
            {
                // Verificar si es un error de clave duplicada
                if (ex.Message.Contains("UNIQUE KEY constraint") && ex.Message.Contains("UQ_Compra_Codigo"))
                {
                    // Intentar regenerar el código y guardar nuevamente
                    try
                    {
                        var nuevoCodigo = GenerarCodigoCompraUnico();
                        var compraRetry = new CompraCreateDto
                        {
                            CodigoCompra = nuevoCodigo,
                            Id_Proveedor = proveedor.Id_Proveedor,
                            FechaCompra = fechaParaEnviar,
                            Detalles = _detalles.Select(d => new DetalleCompraCreateDto
                            {
                                Id_Producto = d.Id_Producto,
                                Cantidad = d.Cantidad,
                                PrecioUnitario = d.PrecioUnitario
                            }).ToList()
                        };

                        lblEstado.Text = "Guardando compra con nuevo código...";
                        Application.DoEvents();

                        var compraCreada = await _compraRepo.CreateAsync(compraRetry);
                        if (compraCreada != null)
                        {
                            // Recargar productos para actualizar el stock
                            lblEstado.Text = "Actualizando información de productos...";
                            Application.DoEvents();
                            
                            try
                            {
                                _productos = await _productoRepo.GetAllAsync();
                                
                                // Actualizar el combo de productos
                                var productoSeleccionado = cmbProducto.SelectedItem as ProductoDto;
                                cmbProducto.DataSource = _productos.Where(p => p.Estado).OrderBy(p => p.Nombre).ToList();
                                cmbProducto.DisplayMember = "Nombre";
                                cmbProducto.ValueMember = "Id_Producto";
                                
                                // Restaurar selección si es posible
                                if (productoSeleccionado != null)
                                {
                                    var nuevoProducto = _productos.FirstOrDefault(p => p.Id_Producto == productoSeleccionado.Id_Producto);
                                    if (nuevoProducto != null && nuevoProducto.Estado)
                                    {
                                        cmbProducto.SelectedValue = nuevoProducto.Id_Producto;
                                        if (cmbProducto.SelectedItem != null)
                                        {
                                            numPrecioUnitario.Value = nuevoProducto.PrecioVenta;
                                        }
                                    }
                                }
                            }
                            catch (Exception exRecargar)
                            {
                                // Si falla la recarga, no es crítico
                                System.Diagnostics.Debug.WriteLine($"Error al recargar productos: {exRecargar.Message}");
                            }

                            MessageBox.Show(
                                $"Compra creada exitosamente.\n\n" +
                                $"Código: {compraCreada.CodigoCompra}\n" +
                                $"Proveedor: {proveedor.Nombre}\n" +
                                $"Fecha: {compraCreada.FechaCompra:dd/MM/yyyy}\n" +
                                $"Total: {lblTotal.Text}\n\n" +
                                $"El stock de los productos ha sido actualizado automáticamente.",
                                "Compra Exitosa", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);

                            // Limpiar formulario
                            _detalles.Clear();
                            ActualizarGridDetalles();
                            numCantidad.Value = 1;
                            
                            if (cmbProducto.Items.Count > 0)
                            {
                                cmbProducto.SelectedIndex = 0;
                            }
                            
                            lblEstado.Text = "Compra guardada exitosamente";
                        }
                        else
                        {
                            MessageBox.Show("No se pudo crear la compra después del reintento. Por favor, verifique los datos e intente nuevamente.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lblEstado.Text = "Error al guardar";
                        }
                    }
                    catch (Exception exRetry)
                    {
                        MessageBox.Show($"Error al guardar compra después de reintento: {exRetry.Message}\n\nPor favor, intente nuevamente.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblEstado.Text = "Error";
                    }
                }
                else
                {
                    MessageBox.Show($"Error al guardar compra: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblEstado.Text = "Error";
                }
            }
            finally
            {
                HabilitarControles(true);
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbProducto_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem != null)
            {
                var producto = cmbProducto.SelectedItem as ProductoDto;
                if (producto != null)
                {
                    numPrecioUnitario.Value = producto.PrecioVenta;
                }
            }
        }
    }
}

