using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class ProveedoresForm : Form
    {
        private readonly ProveedorRepository _proveedorRepo;
        private List<ProveedorDto> _proveedores = new();
        private ProveedorDto? _proveedorSeleccionado;

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public ProveedoresForm()
        {
            InitializeComponent();
            _proveedorRepo = new ProveedorRepository();
            this.Shown += async (s, e) => await CargarProveedores();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvProveedores);
        }

        private async Task CargarProveedores()
        {
            try
            {
                lblEstado.Text = "Cargando proveedores...";
                HabilitarControles(false);

                _proveedores = await _proveedorRepo.GetAllAsync();
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
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGrid()
        {
            dgvProveedores.Rows.Clear();
            foreach (var proveedor in _proveedores.OrderBy(p => p.Nombre))
            {
                dgvProveedores.Rows.Add(
                    proveedor.Id_Proveedor,
                    proveedor.CodigoProveedor,
                    proveedor.Nombre,
                    proveedor.Telefono ?? "",
                    proveedor.Email ?? "",
                    proveedor.Contacto ?? "",
                    proveedor.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void LimpiarFormulario()
        {
            _proveedorSeleccionado = null;
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtContacto.Text = "";
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtContacto.Enabled = habilitar;
            chkEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            dgvProveedores.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
        }

        private void dgvProveedores_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvProveedores.Rows[e.RowIndex].Cells[0].Value);
                _proveedorSeleccionado = _proveedores.FirstOrDefault(p => p.Id_Proveedor == id);

                if (_proveedorSeleccionado != null)
                {
                    txtCodigo.Text = _proveedorSeleccionado.CodigoProveedor;
                    txtNombre.Text = _proveedorSeleccionado.Nombre;
                    txtTelefono.Text = _proveedorSeleccionado.Telefono ?? "";
                    txtEmail.Text = _proveedorSeleccionado.Email ?? "";
                    txtContacto.Text = _proveedorSeleccionado.Contacto ?? "";
                    chkEstado.Checked = _proveedorSeleccionado.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Proveedores");
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
                if (_proveedorSeleccionado == null && !PermissionService.PuedeCrear("Proveedores"))
                {
                    MessageBox.Show("No tiene permisos para crear proveedores.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_proveedorSeleccionado != null && !PermissionService.PuedeActualizar("Proveedores"))
                {
                    MessageBox.Show("No tiene permisos para actualizar proveedores.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblEstado.Text = _proveedorSeleccionado == null ? "Creando proveedor..." : "Actualizando proveedor...";
                HabilitarControles(false);

                if (_proveedorSeleccionado == null)
                {
                    // Crear nuevo
                    var nuevo = new ProveedorCreateDto
                    {
                        CodigoProveedor = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Contacto = string.IsNullOrWhiteSpace(txtContacto.Text) ? null : txtContacto.Text.Trim()
                    };

                    var creado = await _proveedorRepo.CreateAsync(nuevo);
                    if (creado != null)
                    {
                        MessageBox.Show("Proveedor creado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarProveedores();
                    }
                }
                else
                {
                    // Actualizar existente
                    var actualizado = new ProveedorUpdateDto
                    {
                        CodigoProveedor = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Contacto = string.IsNullOrWhiteSpace(txtContacto.Text) ? null : txtContacto.Text.Trim(),
                        Estado = chkEstado.Checked
                    };

                    var ok = await _proveedorRepo.UpdateAsync(_proveedorSeleccionado.Id_Proveedor, actualizado);
                    if (ok)
                    {
                        MessageBox.Show("Proveedor actualizado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarProveedores();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar proveedor: {ex.Message}", "Error",
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
            if (_proveedorSeleccionado == null)
            {
                MessageBox.Show("Seleccione un proveedor para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermissionService.PuedeEliminar("Proveedores"))
            {
                MessageBox.Show("No tiene permisos para eliminar proveedores.", "Acceso Denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar el proveedor '{_proveedorSeleccionado.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    lblEstado.Text = "Eliminando proveedor...";
                    HabilitarControles(false);

                    var ok = await _proveedorRepo.DeleteAsync(_proveedorSeleccionado.Id_Proveedor);
                    if (ok)
                    {
                        MessageBox.Show("Proveedor eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarProveedores();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar proveedor: {ex.Message}", "Error",
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

            dgvProveedores.Rows.Clear();
            var filtrados = _proveedores.Where(p =>
                p.Nombre.ToLower().Contains(busqueda) ||
                p.CodigoProveedor.ToLower().Contains(busqueda) ||
                (p.Telefono?.ToLower().Contains(busqueda) ?? false) ||
                (p.Email?.ToLower().Contains(busqueda) ?? false)
            ).OrderBy(p => p.Nombre);

            foreach (var proveedor in filtrados)
            {
                dgvProveedores.Rows.Add(
                    proveedor.Id_Proveedor,
                    proveedor.CodigoProveedor,
                    proveedor.Nombre,
                    proveedor.Telefono ?? "",
                    proveedor.Email ?? "",
                    proveedor.Contacto ?? "",
                    proveedor.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

