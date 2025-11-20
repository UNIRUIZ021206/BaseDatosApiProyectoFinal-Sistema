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
        private ErrorProvider errorProvider = new ErrorProvider();

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
            
            // Configurar validaciones en tiempo real
            ConfigurarValidaciones();
        }
        
        private void ConfigurarValidaciones()
        {
            // Validar que nombres no acepten números
            txtNombre.KeyPress += TxtNombre_KeyPress;
            txtNombre.Validating += TxtNombre_Validating;
            
            // Validar código
            txtCodigo.Validating += TxtCodigo_Validating;
            
            // Generar código automáticamente al limpiar formulario
            txtCodigo.Enabled = false;
        }
        
        private void TxtNombre_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Permitir teclas de control
            if (char.IsControl(e.KeyChar))
                return;
            
            // No permitir números
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(txtNombre, "Los nombres no pueden contener números.");
                return;
            }
            
            // Permitir letras, espacios, guiones y apóstrofes
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
            {
                e.Handled = true;
                errorProvider.SetError(txtNombre, "Solo se permiten letras, espacios, guiones y apóstrofes.");
            }
            else
            {
                errorProvider.SetError(txtNombre, "");
            }
        }
        
        private void TxtNombre_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider.SetError(txtNombre, "El nombre es obligatorio.");
                e.Cancel = true;
            }
            else if (txtNombre.Text.Trim().Length > 100)
            {
                errorProvider.SetError(txtNombre, "El nombre no puede exceder 100 caracteres.");
                e.Cancel = true;
            }
            else if (txtNombre.Text.Any(char.IsDigit))
            {
                errorProvider.SetError(txtNombre, "El nombre no puede contener números.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtNombre, "");
            }
        }
        
        private void TxtCodigo_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                errorProvider.SetError(txtCodigo, "El código es obligatorio.");
                e.Cancel = true;
            }
            else if (txtCodigo.Text.Trim().Length > 50)
            {
                errorProvider.SetError(txtCodigo, "El código no puede exceder 50 caracteres.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtCodigo, "");
            }
        }
        
        private string GenerarCodigoCategoria()
        {
            var random = new Random(Guid.NewGuid().GetHashCode() ^ Environment.TickCount);
            const string caracteres = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var codigo = new System.Text.StringBuilder(10);
            codigo.Append("CAT"); // Prefijo para categorías
            
            for (int i = 0; i < 7; i++)
            {
                codigo.Append(caracteres[random.Next(caracteres.Length)]);
            }
            
            return codigo.ToString();
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
            // Generar código automáticamente solo al crear nuevo
            txtCodigo.Text = GenerarCodigoCategoria();
            txtCodigo.Enabled = false; // Deshabilitar edición del código
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
            errorProvider.Clear();
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
                    txtCodigo.Enabled = true; // Permitir edición al actualizar
                    txtNombre.Text = _categoriaSeleccionada.Nombre;
                    txtDescripcion.Text = _categoriaSeleccionada.Descripcion ?? "";
                    chkEstado.Checked = _categoriaSeleccionada.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Categorias");
                    errorProvider.Clear();
                }
            }
        }

        private bool ValidarCampos()
        {
            // Validar código
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código de la categoría es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }

            if (txtCodigo.Text.Trim().Length > 50)
            {
                MessageBox.Show("El código no puede exceder 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Trim().Length > 100)
            {
                MessageBox.Show("El nombre no puede exceder 100 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Validar descripción (opcional pero con límite)
            if (!string.IsNullOrWhiteSpace(txtDescripcion.Text) && txtDescripcion.Text.Trim().Length > 500)
            {
                MessageBox.Show("La descripción no puede exceder 500 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return false;
            }

            return true;
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validar todos los campos
                if (!ValidarCampos())
                {
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

