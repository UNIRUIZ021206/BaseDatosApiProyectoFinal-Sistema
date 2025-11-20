using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class PuntoVentaForm : Form
    {
        private readonly ProductoRepository _productoRepo;
        private readonly ClienteRepository _clienteRepo;
        private readonly LoteRepository _loteRepo;
        private readonly FacturaRepository _facturaRepo;
        private readonly SesionRepository _sesionRepo;

        private List<ProductoDto> _productos = new();
        private List<ClienteDto> _clientes = new();
        private List<SesionDto> _sesiones = new();
        private List<ItemCarrito> _carrito = new();
        private SesionDto? _sesionActual;
        private ClienteDto? _clienteActual;

        private class ItemCarrito
        {
            public int Id_Lote { get; set; }
            public int Id_Producto { get; set; }
            public string CodigoProducto { get; set; } = string.Empty;
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

        private string GenerarCodigoFacturaUnico()
        {
            // Usar Guid para obtener una semilla única basada en tiempo y hardware
            // Combinar múltiples fuentes de aleatoriedad para garantizar unicidad
            var random = new Random(Guid.NewGuid().GetHashCode() ^ Environment.TickCount);
            
            // Caracteres permitidos: números (0-9) y letras mayúsculas (A-Z)
            const string caracteres = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            // Generar código de 10 caracteres completamente aleatorio
            // Prefijo "FAC" (3 caracteres) + 7 caracteres aleatorios = 10 caracteres total
            var codigo = new System.Text.StringBuilder(10);
            codigo.Append("FAC"); // Prefijo fijo (3 caracteres)
            
            // Generar 7 caracteres aleatorios alfanuméricos
            for (int i = 0; i < 7; i++)
            {
                codigo.Append(caracteres[random.Next(caracteres.Length)]);
            }
            
            return codigo.ToString(); // Total: 10 caracteres (FAC + 7 aleatorios)
        }

        public PuntoVentaForm()
        {
            InitializeComponent();
            
            // Validar permisos de seguridad antes de continuar
            if (!PermissionService.PuedeUsarPuntoVenta())
            {
                MessageBox.Show("No tiene permisos para usar el Punto de Venta.\n\nSolo Administradores y Cajeros pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            
            _productoRepo = new ProductoRepository();
            _clienteRepo = new ClienteRepository();
            _loteRepo = new LoteRepository();
            _facturaRepo = new FacturaRepository();
            _sesionRepo = new SesionRepository();

            // Cargar datos después de que el formulario se muestre
            this.Shown += async (s, e) => await CargarDatos();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvProductos);
            DataGridViewHelper.AplicarEstiloModerno(dgvCarrito);
        }

        private async Task CargarDatos()
        {
            try
            {
                lblEstado.Text = "Cargando datos...";
                // Deshabilitar solo los controles, no el formulario completo
                HabilitarControles(false);
                Application.DoEvents();

                // Cargar datos en paralelo para mayor velocidad
                var productosTask = _productoRepo.GetAllAsync();
                var clientesTask = _clienteRepo.GetAllAsync();
                var sesionesTask = _sesionRepo.GetActivasAsync();

                // Esperar todas las tareas en paralelo
                await Task.WhenAll(productosTask, clientesTask, sesionesTask);

                _productos = await productosTask;
                _clientes = await clientesTask;
                _sesiones = await sesionesTask;

                // Seleccionar primera sesión activa
                if (_sesiones.Any())
                {
                    _sesionActual = _sesiones.First();
                    lblSesion.Text = $"Sesión: {_sesionActual.CodigoSesion}";
                }
                else
                {
                    var resultado = MessageBox.Show(
                        "No hay sesiones activas. ¿Desea abrir una nueva sesión de caja ahora?",
                        "Sesión Requerida",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        // Abrir formulario de sesiones
                        using (var sesionForm = new SesionCajaForm())
                        {
                            if (sesionForm.ShowDialog() == DialogResult.OK)
                            {
                                // Recargar sesiones después de abrir una nueva
                                _sesiones = await _sesionRepo.GetActivasAsync();
                                if (_sesiones.Any())
                                {
                                    _sesionActual = _sesiones.First();
                                    lblSesion.Text = $"Sesión: {_sesionActual.CodigoSesion}";
                                }
                                else
                                {
                                    this.Close();
                                    return;
                                }
                            }
                            else
                            {
                                this.Close();
                                return;
                            }
                        }
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }

                // Cargar clientes en combo
                cmbCliente.Items.Clear();
                cmbCliente.DisplayMember = "NombreCompleto";
                cmbCliente.ValueMember = "Id_Cliente";
                
                // Agregar "Cliente General" primero
                cmbCliente.Items.Add("Cliente General");
                
                // Agregar clientes activos
                foreach (var cliente in _clientes.Where(c => c.Estado).OrderBy(c => c.NombreCompleto))
                {
                    cmbCliente.Items.Add(cliente);
                }
                
                cmbCliente.SelectedIndex = 0;

                ActualizarGridProductos();
                ActualizarCarrito();
                lblEstado.Text = "Listo";
                HabilitarControles(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message + "\n\nSerá redirigido al login.", "Sesión Expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                HabilitarControles(true);
            }
        }

        private void HabilitarControles(bool habilitar)
        {
            dgvProductos.Enabled = habilitar;
            dgvCarrito.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
            cmbCliente.Enabled = habilitar;
            btnEliminarItem.Enabled = habilitar;
            btnLimpiarCarrito.Enabled = habilitar;
            btnFinalizarVenta.Enabled = habilitar;
        }

        private void ActualizarGridProductos()
        {
            dgvProductos.Rows.Clear();
            foreach (var producto in _productos.Where(p => p.Estado && p.StockActual > 0))
            {
                // Calcular stock disponible: stock actual menos lo que está en el carrito del usuario actual
                var cantidadEnCarrito = _carrito
                    .Where(c => c.Id_Producto == producto.Id_Producto)
                    .Sum(c => c.Cantidad);
                var stockDisponible = producto.StockActual - cantidadEnCarrito;
                
                // Solo mostrar productos con stock disponible > 0
                if (stockDisponible > 0)
            {
                dgvProductos.Rows.Add(
                    producto.CodigoProducto ?? string.Empty,
                    producto.Nombre ?? string.Empty,
                    producto.NombreCategoria ?? string.Empty,
                    producto.PrecioVenta.ToString("C"),
                        stockDisponible // Mostrar stock disponible (actual - en carrito)
                );
                }
            }
        }

        private void ActualizarCarrito()
        {
            dgvCarrito.Rows.Clear();
            foreach (var item in _carrito)
            {
                dgvCarrito.Rows.Add(
                    item.CodigoProducto ?? string.Empty,
                    item.NombreProducto ?? string.Empty,
                    item.Cantidad,
                    item.PrecioUnitario.ToString("C"),
                    item.Subtotal.ToString("C")
                );
            }

            var subtotal = _carrito.Sum(i => i.Subtotal);
            var descuento = 0m;
            var impuesto = subtotal * 0.15m; // 15% de impuesto
            var total = subtotal - descuento + impuesto;

            lblSubtotal.Text = subtotal.ToString("C");
            lblDescuento.Text = descuento.ToString("C");
            lblImpuesto.Text = impuesto.ToString("C");
            lblTotal.Text = total.ToString("C");
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            var busqueda = txtBuscar.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(busqueda))
            {
                ActualizarGridProductos();
                return;
            }

            dgvProductos.Rows.Clear();
            var productosFiltrados = _productos.Where(p =>
                p.Estado &&
                p.StockActual > 0 &&
                (p.CodigoProducto.ToLower().Contains(busqueda) ||
                 p.Nombre.ToLower().Contains(busqueda) ||
                 p.NombreCategoria?.ToLower().Contains(busqueda) == true)
            );

            foreach (var producto in productosFiltrados)
            {
                dgvProductos.Rows.Add(
                    producto.CodigoProducto ?? string.Empty,
                    producto.Nombre ?? string.Empty,
                    producto.NombreCategoria ?? string.Empty,
                    producto.PrecioVenta.ToString("C"),
                    producto.StockActual
                );
            }
        }

        private async void dgvProductos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProductos.Rows.Count) return;

            try
            {
                var codigoProducto = dgvProductos.Rows[e.RowIndex].Cells[0].Value?.ToString();
                if (string.IsNullOrEmpty(codigoProducto))
                {
                    MessageBox.Show("No se pudo obtener el código del producto.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var producto = _productos.FirstOrDefault(p => 
                    p.CodigoProducto != null && 
                    p.CodigoProducto.Equals(codigoProducto, StringComparison.OrdinalIgnoreCase));
                
                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (producto.StockActual <= 0)
                {
                    MessageBox.Show("No hay stock disponible para este producto.", "Sin Stock",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblEstado.Text = "Buscando lotes disponibles...";
                HabilitarControles(false);
                Application.DoEvents();

                // Obtener lotes disponibles
                var lotes = await _loteRepo.GetLotesDisponiblesPorProductoAsync(producto.Id_Producto);
                
                HabilitarControles(true);
                
                if (!lotes.Any())
                {
                    MessageBox.Show("No hay lotes disponibles para este producto.", "Sin Stock",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblEstado.Text = "Listo";
                    return;
                }

                // Ordenar lotes: primero los que vencen antes, luego los que tienen más stock
                var lotesOrdenados = lotes
                    .OrderBy(l => l.FechaVencimiento ?? DateTime.MaxValue)
                    .ThenByDescending(l => l.Cantidad)
                    .ToList();

                // Calcular stock total disponible (suma de todos los lotes)
                var stockDisponible = lotesOrdenados.Sum(l => l.Cantidad);
                var stockMaximo = Math.Min(producto.StockActual, stockDisponible);

                // Pedir cantidad
                using var formCantidad = new Form
                {
                    Text = $"Agregar {producto.Nombre}",
                    Size = new System.Drawing.Size(350, 180),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var lblProducto = new Label 
                { 
                    Text = $"Producto: {producto.Nombre}\nPrecio: {producto.PrecioVenta:C}\nStock disponible: {stockMaximo}",
                    Location = new System.Drawing.Point(20, 10),
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 9F)
                };
                
                var lblCantidad = new Label 
                { 
                    Text = "Cantidad:", 
                    Location = new System.Drawing.Point(20, 80), 
                    AutoSize = true 
                };
                
                var numCantidad = new NumericUpDown
                {
                    Location = new System.Drawing.Point(20, 100),
                    Width = 300,
                    Minimum = 1,
                    Maximum = stockMaximo,
                    Value = 1
                };
                
                var btnAceptar = new Button
                {
                    Text = "Agregar al Carrito",
                    Location = new System.Drawing.Point(20, 130),
                    Width = 140,
                    DialogResult = DialogResult.OK,
                    BackColor = VerdePrincipal,
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = System.Windows.Forms.FlatStyle.Flat
                };
                
                var btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new System.Drawing.Point(170, 130),
                    Width = 140,
                    DialogResult = DialogResult.Cancel,
                    BackColor = Alerta,
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = System.Windows.Forms.FlatStyle.Flat
                };

                formCantidad.Controls.AddRange(new Control[] { lblProducto, lblCantidad, numCantidad, btnAceptar, btnCancelar });
                formCantidad.AcceptButton = btnAceptar;
                formCantidad.CancelButton = btnCancelar;

                if (formCantidad.ShowDialog() == DialogResult.OK)
                {
                    var cantidad = (int)numCantidad.Value;
                    
                    if (cantidad <= 0 || cantidad > stockMaximo)
                    {
                        MessageBox.Show($"La cantidad debe estar entre 1 y {stockMaximo}.", "Cantidad Inválida",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblEstado.Text = "Listo";
                        return;
                    }

                    // Seleccionar lotes para cumplir con la cantidad solicitada
                    // Usar primero los lotes que vencen antes, luego los que tienen más stock
                    var cantidadRestante = cantidad;
                    var lotesSeleccionados = new List<(int Id_Lote, int Cantidad)>();

                    foreach (var lote in lotesOrdenados)
                    {
                        if (cantidadRestante <= 0) break;

                        // Verificar si ya existe este lote en el carrito
                    var itemExistente = _carrito.FirstOrDefault(i => i.Id_Lote == lote.Id_Lote);
                        var cantidadYaEnCarrito = itemExistente?.Cantidad ?? 0;
                        var cantidadDisponibleEnLote = lote.Cantidad - cantidadYaEnCarrito;

                        if (cantidadDisponibleEnLote > 0)
                        {
                            var cantidadAUsar = Math.Min(cantidadRestante, cantidadDisponibleEnLote);
                            lotesSeleccionados.Add((lote.Id_Lote, cantidadAUsar));
                            cantidadRestante -= cantidadAUsar;
                        }
                    }

                    // Verificar que se pudo cubrir toda la cantidad solicitada
                    if (cantidadRestante > 0)
                    {
                        var stockRealDisponible = lotesOrdenados.Sum(l => 
                        {
                            var itemExistente = _carrito.FirstOrDefault(i => i.Id_Lote == l.Id_Lote);
                            var cantidadYaEnCarrito = itemExistente?.Cantidad ?? 0;
                            return Math.Max(0, l.Cantidad - cantidadYaEnCarrito);
                        });

                        MessageBox.Show($"No hay suficiente stock disponible. Stock disponible real: {stockRealDisponible}, solicitado: {cantidad}",
                                "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            lblEstado.Text = "Listo";
                            return;
                        }

                    // Agregar o actualizar items en el carrito
                    foreach (var (idLote, cantidadLote) in lotesSeleccionados)
                    {
                        var itemExistente = _carrito.FirstOrDefault(i => i.Id_Lote == idLote);
                        
                        if (itemExistente != null)
                        {
                            itemExistente.Cantidad += cantidadLote;
                        }
                        else
                        {
                        _carrito.Add(new ItemCarrito
                        {
                                Id_Lote = idLote,
                            Id_Producto = producto.Id_Producto,
                            CodigoProducto = producto.CodigoProducto ?? string.Empty,
                            NombreProducto = producto.Nombre ?? string.Empty,
                                Cantidad = cantidadLote,
                            PrecioUnitario = producto.PrecioVenta
                        });
                        }
                    }

                    ActualizarCarrito();
                    // Actualizar el grid de productos para reflejar el stock disponible actualizado
                    ActualizarGridProductos();
                    lblEstado.Text = $"Producto agregado: {producto.Nombre} x{cantidad}";
                }
                else
                {
                    lblEstado.Text = "Listo";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message + "\n\nSerá redirigido al login.", "Sesión Expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto al carrito: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Listo";
                HabilitarControles(true);
            }
        }

        private void btnEliminarItem_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0)
            {
                var index = dgvCarrito.SelectedRows[0].Index;
                if (index < _carrito.Count)
                {
                    _carrito.RemoveAt(index);
                    ActualizarCarrito();
                    // Actualizar el grid de productos para reflejar el stock disponible actualizado
                    ActualizarGridProductos();
                }
            }
        }

        private async void btnFinalizarVenta_Click(object? sender, EventArgs e)
        {
            if (!_carrito.Any())
            {
                MessageBox.Show("El carrito está vacío.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_sesionActual == null)
            {
                MessageBox.Show("No hay sesión activa.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var resultado = MessageBox.Show(
                    $"¿Confirmar venta por {lblTotal.Text}?",
                    "Confirmar Venta",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes) return;

                lblEstado.Text = "Validando stock...";
                btnFinalizarVenta.Enabled = false;

                // Validar stock antes de procesar la venta
                // Agrupar items por lote y sumar cantidades para validar correctamente
                var itemsSinStock = new List<string>();
                var itemsPorLote = _carrito
                    .GroupBy(i => i.Id_Lote)
                    .Select(g => new 
                    { 
                        Id_Lote = g.Key, 
                        CantidadTotal = g.Sum(i => i.Cantidad),
                        PrimerItem = g.First(),
                        Items = g.ToList()
                    })
                    .ToList();
                
                // Recargar lotes justo antes de validar para tener la información más actualizada
                var lotesCache = new Dictionary<int, List<LoteDto>>();
                
                foreach (var itemLote in itemsPorLote)
                {
                    try
                    {
                        var idLote = itemLote.Id_Lote;
                        var cantidadTotalEnLote = itemLote.CantidadTotal;
                        var primerItem = itemLote.PrimerItem;
                        
                        // Obtener lotes del producto (usar cache si ya los obtuvimos)
                        if (!lotesCache.ContainsKey(primerItem.Id_Producto))
                        {
                            lotesCache[primerItem.Id_Producto] = await _loteRepo.GetLotesDisponiblesPorProductoAsync(primerItem.Id_Producto);
                        }
                        
                        var lotesDisponibles = lotesCache[primerItem.Id_Producto];
                        var loteActual = lotesDisponibles.FirstOrDefault(l => l.Id_Lote == idLote);
                        
                        if (loteActual == null)
                        {
                            var producto = _productos.FirstOrDefault(p => p.Id_Producto == primerItem.Id_Producto);
                            var nombreProducto = producto?.Nombre ?? "Producto desconocido";
                            itemsSinStock.Add($"{nombreProducto} (Lote ID {idLote}): El lote ya no existe, fue eliminado o está inactivo");
                            continue;
                        }
                        
                        if (!loteActual.Estado)
                        {
                            var producto = _productos.FirstOrDefault(p => p.Id_Producto == primerItem.Id_Producto);
                            var nombreProducto = producto?.Nombre ?? "Producto desconocido";
                            itemsSinStock.Add($"{nombreProducto} (Lote {loteActual.CodigoLote}): El lote está inactivo o vencido");
                            continue;
                        }
                        
                        // Validar que la cantidad total no exceda el stock disponible
                        if (loteActual.Cantidad < cantidadTotalEnLote)
                        {
                            var producto = _productos.FirstOrDefault(p => p.Id_Producto == primerItem.Id_Producto);
                            var nombreProducto = producto?.Nombre ?? "Producto desconocido";
                            var cantidadEnCarrito = itemLote.Items.Sum(i => i.Cantidad);
                            itemsSinStock.Add($"{nombreProducto} (Lote {loteActual.CodigoLote}): Stock disponible {loteActual.Cantidad}, solicitado {cantidadTotalEnLote}");
                        }
                    }
                    catch (Exception ex)
                    {
                        var producto = _productos.FirstOrDefault(p => p.Id_Producto == itemLote.PrimerItem.Id_Producto);
                        var nombreProducto = producto?.Nombre ?? "Producto desconocido";
                        itemsSinStock.Add($"{nombreProducto} (Lote ID {itemLote.Id_Lote}): Error al verificar el stock - {ex.Message}");
                    }
                }

                if (itemsSinStock.Any())
                {
                    var mensaje = "Los siguientes productos no tienen stock suficiente:\n\n" +
                                 string.Join("\n", itemsSinStock) +
                                 "\n\nPor favor, ajuste las cantidades o elimine los productos del carrito.";
                    MessageBox.Show(mensaje, "Stock Insuficiente",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblEstado.Text = "Listo";
                    btnFinalizarVenta.Enabled = true;
                    return;
                }

                lblEstado.Text = "Procesando venta...";

                // Agrupar items del carrito SOLO por lote (no por precio)
                // Si hay múltiples items del mismo lote, sumar las cantidades
                // Si hay diferentes precios para el mismo lote, calcular precio promedio ponderado
                var detallesAgrupados = _carrito
                    .GroupBy(i => i.Id_Lote)
                    .Select(g => 
                    {
                        var totalCantidad = g.Sum(i => i.Cantidad);
                        var totalSubtotal = g.Sum(i => i.Subtotal);
                        // Calcular precio promedio ponderado si hay múltiples precios para el mismo lote
                        var precioPromedio = totalCantidad > 0 ? totalSubtotal / totalCantidad : g.First().PrecioUnitario;
                        
                        return new DetalleFacturaCreateDto
                        {
                            Id_Lote = g.Key,
                            Cantidad = totalCantidad,
                            PrecioUnitario = precioPromedio
                        };
                    })
                    .ToList();
                
                // Log para diagnóstico (puedes comentar esto después)
                System.Diagnostics.Debug.WriteLine($"Detalles agrupados a enviar: {detallesAgrupados.Count}");
                foreach (var det in detallesAgrupados)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Lote ID: {det.Id_Lote}, Cantidad: {det.Cantidad}, Precio: {det.PrecioUnitario}");
                }

                // Crear factura con código único de 10 caracteres (FAC + 7 aleatorios)
                var codigoUnico = GenerarCodigoFacturaUnico();
                var factura = new FacturaCreateDto
                {
                    CodigoFactura = codigoUnico,
                    NumeroFactura = codigoUnico,
                    Id_Cliente = _clienteActual?.Id_Cliente ?? null,
                    Id_Sesion = _sesionActual.Id_Sesion,
                    Subtotal = _carrito.Sum(i => i.Subtotal),
                    Descuento = 0,
                    Impuesto = _carrito.Sum(i => i.Subtotal) * 0.15m,
                    Detalles = detallesAgrupados
                };

                var facturaCreada = await _facturaRepo.CreateAsync(factura);

                if (facturaCreada != null)
                {
                    MessageBox.Show(
                        $"Venta realizada exitosamente.\nFactura: {facturaCreada.NumeroFactura}\nTotal: {facturaCreada.Total:C}",
                        "Venta Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Limpiar carrito
                    _carrito.Clear();
                    ActualizarCarrito();
                    txtBuscar.Clear();
                    ActualizarGridProductos();

                    // Recargar productos para actualizar stock
                    _productos = await _productoRepo.GetAllAsync();
                    ActualizarGridProductos();
                }
            }
            catch (Exception ex)
            {
                // Mejorar el mensaje de error para restricciones CHECK y problemas de stock
                string mensajeError = ex.Message;
                bool esErrorStock = false;
                
                // El stored procedure ahora envía mensajes descriptivos con RAISERROR
                // Priorizar el mensaje del InnerException si viene del SP
                string mensajeOriginal = ex.Message;
                if (ex.InnerException != null)
                {
                    var innerMsg = ex.InnerException.Message;
                    // Si el InnerException tiene un mensaje más específico, usarlo
                    if (innerMsg.Contains("Stock insuficiente") || innerMsg.Contains("lote") || 
                        innerMsg.Contains("Lote") || innerMsg.Contains("stock") || innerMsg.Contains("Stock") ||
                        innerMsg.Contains("insufficient"))
                    {
                        esErrorStock = true;
                        mensajeOriginal = innerMsg; // Usar el mensaje del SP directamente
                    }
                }
                
                // También verificar el mensaje principal (puede venir directamente del SP)
                if (mensajeOriginal.Contains("Stock insuficiente") || mensajeOriginal.Contains("lote") || 
                    mensajeOriginal.Contains("Lote") || mensajeOriginal.Contains("stock") || 
                    mensajeOriginal.Contains("Stock") || mensajeOriginal.Contains("insufficient"))
                {
                    esErrorStock = true;
                }
                
                // Detectar otros tipos de errores de stock
                if (!esErrorStock && (ex.Message.Contains("CHECK constraint") || ex.Message.Contains("CHK_") || 
                    (ex.Message.Contains("BadRequest") && (ex.Message.Contains("stock") || ex.Message.Contains("Stock")))))
                {
                    esErrorStock = true;
                }
                
                // Construir el mensaje final
                if (esErrorStock)
                {
                    // Si el mensaje original es descriptivo (viene del SP), usarlo directamente
                    if (mensajeOriginal.Contains("Stock insuficiente") || mensajeOriginal.Contains("producto") || 
                        mensajeOriginal.Contains("Lote:") || mensajeOriginal.Contains("Lote ") ||
                        mensajeOriginal.Contains("disponible:") || mensajeOriginal.Contains("solicitado:"))
                    {
                        mensajeError = $"Error al procesar la venta:\n\n{mensajeOriginal}\n\n" +
                                      "Se recargarán los productos para mostrar el stock actualizado.";
                    }
                    else
                    {
                        // Mostrar información de diagnóstico si el mensaje no es descriptivo
                        var detalleError = $"Mensaje de error: {mensajeOriginal}";
                        if (ex.InnerException != null)
                        {
                            detalleError += $"\n\nError interno: {ex.InnerException.Message}";
                        }
                        
                    mensajeError = "Error: No hay suficiente stock disponible para completar la venta.\n\n" +
                                      "Esto puede ocurrir si:\n" +
                                      "• Otro usuario vendió productos mientras procesaba la venta\n" +
                                      "• El stock cambió entre la validación y el procesamiento\n\n" +
                                      $"{detalleError}\n\n" +
                                      "Se recargarán los productos para mostrar el stock actualizado.\n" +
                                      "Por favor, verifique el stock y ajuste las cantidades si es necesario.";
                    }
                }
                else
                {
                    // Si no es error de stock, mostrar el mensaje completo para diagnóstico
                    mensajeError = $"Error al procesar la venta:\n\n{ex.Message}";
                    if (ex.InnerException != null)
                    {
                        mensajeError += $"\n\nError interno: {ex.InnerException.Message}";
                    }
                }
                
                // Recargar productos si es un error de stock
                if (esErrorStock)
                {
                    try
                    {
                        lblEstado.Text = "Recargando productos...";
                        Application.DoEvents();
                        _productos = await _productoRepo.GetAllAsync();
                        ActualizarGridProductos();
                    }
                    catch
                    {
                        // Si falla la recarga, continuar con el mensaje de error
                    }
                }

                MessageBox.Show($"Error al procesar la venta:\n\n{mensajeError}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblEstado.Text = "Listo";
                btnFinalizarVenta.Enabled = true;
            }
        }

        private void cmbCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbCliente.SelectedIndex > 0 && cmbCliente.SelectedItem is ClienteDto cliente)
            {
                _clienteActual = cliente;
            }
            else
            {
                _clienteActual = null;
            }
        }

        private void btnLimpiarCarrito_Click(object? sender, EventArgs e)
        {
            if (_carrito.Any())
            {
                var resultado = MessageBox.Show("¿Limpiar el carrito?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    _carrito.Clear();
                    ActualizarCarrito();
                    // Actualizar el grid de productos para reflejar el stock disponible actualizado
                    ActualizarGridProductos();
                }
            }
        }

        private void dgvProductos_KeyDown(object? sender, KeyEventArgs e)
        {
            // Permitir agregar al carrito con Enter
            if (e.KeyCode == Keys.Enter && dgvProductos.SelectedRows.Count > 0)
            {
                var rowIndex = dgvProductos.SelectedRows[0].Index;
                var cellEventArgs = new DataGridViewCellEventArgs(0, rowIndex);
                dgvProductos_CellDoubleClick(sender, cellEventArgs);
                e.Handled = true;
            }
        }
    }
}
