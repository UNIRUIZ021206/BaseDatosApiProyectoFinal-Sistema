using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class CategoriasForm : Form
    {
        private readonly CategoriaRepository _categoriaRepo;
        private List<CategoriaDto> _categorias = new();
        private CategoriaDto? _categoriaSeleccionada;

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public CategoriasForm()
        {
            InitializeComponent();
            _categoriaRepo = new CategoriaRepository();
            this.Shown += async (s, e) => await CargarCategorias();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvCategorias);
        }

        private async Task CargarCategorias()
        {
            try
            {
                lblEstado.Text = "Cargando categorías...";
                HabilitarControles(false);

                _categorias = await _categoriaRepo.GetAllAsync();
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
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGrid()
        {
            dgvCategorias.Rows.Clear();
            foreach (var categoria in _categorias.OrderBy(c => c.Nombre))
            {
                dgvCategorias.Rows.Add(
                    categoria.Id_Categoria,
                    categoria.CodigoCategoria,
                    categoria.Nombre,
                    categoria.Descripcion ?? "",
                    categoria.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void LimpiarFormulario()
        {
            _categoriaSeleccionada = null;
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
            chkEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            dgvCategorias.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
        }

        private void dgvCategorias_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvCategorias.Rows[e.RowIndex].Cells[0].Value);
                _categoriaSeleccionada = _categorias.FirstOrDefault(c => c.Id_Categoria == id);

                if (_categoriaSeleccionada != null)
                {
                    txtCodigo.Text = _categoriaSeleccionada.CodigoCategoria;
                    txtNombre.Text = _categoriaSeleccionada.Nombre;
                    txtDescripcion.Text = _categoriaSeleccionada.Descripcion ?? "";
                    chkEstado.Checked = _categoriaSeleccionada.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Categorias");
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

                // Verificar permisos
                if (_categoriaSeleccionada == null && !PermissionService.PuedeCrear("Categorias"))
                {
                    MessageBox.Show("No tiene permisos para crear categorías.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_categoriaSeleccionada != null && !PermissionService.PuedeActualizar("Categorias"))
                {
                    MessageBox.Show("No tiene permisos para actualizar categorías.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblEstado.Text = _categoriaSeleccionada == null ? "Creando categoría..." : "Actualizando categoría...";
                HabilitarControles(false);

                if (_categoriaSeleccionada == null)
                {
                    // Crear nuevo
                    var nuevo = new CategoriaCreateDto
                    {
                        CodigoCategoria = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim()
                    };

                    var creado = await _categoriaRepo.CreateAsync(nuevo);
                    if (creado != null)
                    {
                        MessageBox.Show("Categoría creada exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarCategorias();
                    }
                }
                else
                {
                    // Actualizar existente
                    var actualizado = new CategoriaUpdateDto
                    {
                        CodigoCategoria = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                        Estado = chkEstado.Checked
                    };

                    var ok = await _categoriaRepo.UpdateAsync(_categoriaSeleccionada.Id_Categoria, actualizado);
                    if (ok)
                    {
                        MessageBox.Show("Categoría actualizada exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarCategorias();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar categoría: {ex.Message}", "Error",
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
            if (_categoriaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una categoría para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermissionService.PuedeEliminar("Categorias"))
            {
                MessageBox.Show("No tiene permisos para eliminar categorías.", "Acceso Denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar la categoría '{_categoriaSeleccionada.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    lblEstado.Text = "Eliminando categoría...";
                    HabilitarControles(false);

                    var ok = await _categoriaRepo.DeleteAsync(_categoriaSeleccionada.Id_Categoria);
                    if (ok)
                    {
                        MessageBox.Show("Categoría eliminada exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarCategorias();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar categoría: {ex.Message}", "Error",
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

            dgvCategorias.Rows.Clear();
            var filtradas = _categorias.Where(c =>
                c.Nombre.ToLower().Contains(busqueda) ||
                c.CodigoCategoria.ToLower().Contains(busqueda) ||
                (c.Descripcion?.ToLower().Contains(busqueda) ?? false)
            ).OrderBy(c => c.Nombre);

            foreach (var categoria in filtradas)
            {
                dgvCategorias.Rows.Add(
                    categoria.Id_Categoria,
                    categoria.CodigoCategoria,
                    categoria.Nombre,
                    categoria.Descripcion ?? "",
                    categoria.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

