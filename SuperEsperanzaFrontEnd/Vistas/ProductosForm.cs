using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class ProductosForm : Form
    {
        private readonly ProductoRepository _productoRepo;
        private readonly CategoriaRepository _categoriaRepo;
        private List<ProductoDto> _productos = new();
        private List<CategoriaDto> _categorias = new();
        private ProductoDto? _productoSeleccionado;

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public ProductosForm()
        {
            InitializeComponent();
            _productoRepo = new ProductoRepository();
            _categoriaRepo = new CategoriaRepository();
            this.Shown += async (s, e) => await CargarDatos();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvProductos);
        }

        private async Task CargarDatos()
        {
            try
            {
                lblEstado.Text = "Cargando datos...";
                HabilitarControles(false);

                var productosTask = _productoRepo.GetAllAsync();
                var categoriasTask = _categoriaRepo.GetAllAsync();

                await Task.WhenAll(productosTask, categoriasTask);

                _productos = await productosTask;
                _categorias = await categoriasTask;

                CargarCategorias();
                ActualizarGrid();
                LimpiarFormulario();

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
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void CargarCategorias()
        {
            cmbCategoria.Items.Clear();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "Id_Categoria";
            
            foreach (var categoria in _categorias.Where(c => c.Estado).OrderBy(c => c.Nombre))
            {
                cmbCategoria.Items.Add(categoria);
            }
        }

        private void ActualizarGrid()
        {
            dgvProductos.Rows.Clear();
            foreach (var producto in _productos.OrderBy(p => p.Nombre))
            {
                dgvProductos.Rows.Add(
                    producto.Id_Producto,
                    producto.CodigoProducto,
                    producto.Nombre,
                    producto.NombreCategoria ?? "",
                    producto.PrecioVenta.ToString("C"),
                    producto.StockActual,
                    producto.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void LimpiarFormulario()
        {
            _productoSeleccionado = null;
            txtCodigo.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            numPrecioVenta.Value = 0;
            numStockActual.Value = 0;
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            cmbCategoria.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
            numPrecioVenta.Enabled = habilitar;
            numStockActual.Enabled = habilitar;
            chkEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            dgvProductos.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
        }

        private void dgvProductos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells[0].Value);
                _productoSeleccionado = _productos.FirstOrDefault(p => p.Id_Producto == id);

                if (_productoSeleccionado != null)
                {
                    txtCodigo.Text = _productoSeleccionado.CodigoProducto;
                    
                    // Seleccionar categoría
                    for (int i = 0; i < cmbCategoria.Items.Count; i++)
                    {
                        if (cmbCategoria.Items[i] is CategoriaDto cat && cat.Id_Categoria == _productoSeleccionado.Id_Categoria)
                        {
                            cmbCategoria.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    txtNombre.Text = _productoSeleccionado.Nombre;
                    txtDescripcion.Text = _productoSeleccionado.Descripcion ?? "";
                    numPrecioVenta.Value = _productoSeleccionado.PrecioVenta;
                    numStockActual.Value = _productoSeleccionado.StockActual;
                    chkEstado.Checked = _productoSeleccionado.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Productos");
                }
            }
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El código y el nombre son obligatorios.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbCategoria.SelectedIndex < 0 || cmbCategoria.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numPrecioVenta.Value <= 0)
                {
                    MessageBox.Show("El precio de venta debe ser mayor a cero.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar permisos
                if (_productoSeleccionado == null && !PermissionService.PuedeCrear("Productos"))
                {
                    MessageBox.Show("No tiene permisos para crear productos.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_productoSeleccionado != null && !PermissionService.PuedeActualizar("Productos"))
                {
                    MessageBox.Show("No tiene permisos para actualizar productos.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var categoriaSeleccionada = cmbCategoria.SelectedItem as CategoriaDto;
                if (categoriaSeleccionada == null)
                {
                    MessageBox.Show("Categoría inválida.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblEstado.Text = _productoSeleccionado == null ? "Creando producto..." : "Actualizando producto...";
                HabilitarControles(false);

                if (_productoSeleccionado == null)
                {
                    // Crear nuevo
                    var nuevo = new ProductoCreateDto
                    {
                        CodigoProducto = txtCodigo.Text.Trim(),
                        Id_Categoria = categoriaSeleccionada.Id_Categoria,
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                        PrecioVenta = numPrecioVenta.Value,
                        StockActual = (int)numStockActual.Value
                    };

                    var creado = await _productoRepo.CreateAsync(nuevo);
                    if (creado != null)
                    {
                        MessageBox.Show("Producto creado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
                else
                {
                    // Actualizar existente
                    var actualizado = new ProductoUpdateDto
                    {
                        CodigoProducto = txtCodigo.Text.Trim(),
                        Id_Categoria = categoriaSeleccionada.Id_Categoria,
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                        PrecioVenta = numPrecioVenta.Value,
                        StockActual = (int)numStockActual.Value,
                        Estado = chkEstado.Checked
                    };

                    var ok = await _productoRepo.UpdateAsync(_productoSeleccionado.Id_Producto, actualizado);
                    if (ok)
                    {
                        MessageBox.Show("Producto actualizado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar producto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
            }
            finally
            {
                HabilitarControles(true);
            }
        }

        private async void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_productoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermissionService.PuedeEliminar("Productos"))
            {
                MessageBox.Show("No tiene permisos para eliminar productos.", "Acceso Denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar el producto '{_productoSeleccionado.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    lblEstado.Text = "Eliminando producto...";
                    HabilitarControles(false);

                    var ok = await _productoRepo.DeleteAsync(_productoSeleccionado.Id_Producto);
                    if (ok)
                    {
                        MessageBox.Show("Producto eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblEstado.Text = "Error";
                }
                finally
                {
                    HabilitarControles(true);
                }
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            var busqueda = txtBuscar.Text.ToLower();
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                ActualizarGrid();
                return;
            }

            dgvProductos.Rows.Clear();
            var filtrados = _productos.Where(p =>
                p.Nombre.ToLower().Contains(busqueda) ||
                p.CodigoProducto.ToLower().Contains(busqueda) ||
                (p.NombreCategoria?.ToLower().Contains(busqueda) ?? false) ||
                (p.Descripcion?.ToLower().Contains(busqueda) ?? false)
            ).OrderBy(p => p.Nombre);

            foreach (var producto in filtrados)
            {
                dgvProductos.Rows.Add(
                    producto.Id_Producto,
                    producto.CodigoProducto,
                    producto.Nombre,
                    producto.NombreCategoria ?? "",
                    producto.PrecioVenta.ToString("C"),
                    producto.StockActual,
                    producto.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

