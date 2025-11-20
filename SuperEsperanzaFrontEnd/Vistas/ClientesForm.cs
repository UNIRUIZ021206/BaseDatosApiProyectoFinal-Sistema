using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class ClientesForm : Form
    {
        private readonly ClienteRepository _clienteRepo;
        private List<ClienteDto> _clientes = new();
        private ClienteDto? _clienteSeleccionado;

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public ClientesForm()
        {
            InitializeComponent();
            _clienteRepo = new ClienteRepository();
            this.Shown += async (s, e) => await CargarClientes();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvClientes);
        }

        private async Task CargarClientes()
        {
            try
            {
                lblEstado.Text = "Cargando clientes...";
                HabilitarControles(false);

                _clientes = await _clienteRepo.GetAllAsync();
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
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGrid()
        {
            dgvClientes.Rows.Clear();
            foreach (var cliente in _clientes.OrderBy(c => c.NombreCompleto))
            {
                dgvClientes.Rows.Add(
                    cliente.Id_Cliente,
                    cliente.CodigoCliente,
                    cliente.NombreCompleto,
                    cliente.Telefono ?? "",
                    cliente.Email ?? "",
                    cliente.TipoMembresia ?? "",
                    cliente.PuntosCompra,
                    cliente.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void LimpiarFormulario()
        {
            _clienteSeleccionado = null;
            txtCodigo.Text = "";
            txtPNombre.Text = "";
            txtSNombre.Text = "";
            txtPApellido.Text = "";
            txtSApellido.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            cmbTipoMembresia.SelectedIndex = -1;
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtPNombre.Enabled = habilitar;
            txtSNombre.Enabled = habilitar;
            txtPApellido.Enabled = habilitar;
            txtSApellido.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;
            cmbTipoMembresia.Enabled = habilitar;
            chkEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            dgvClientes.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
        }

        private void dgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                _clienteSeleccionado = _clientes.FirstOrDefault(c => c.Id_Cliente == id);

                if (_clienteSeleccionado != null)
                {
                    txtCodigo.Text = _clienteSeleccionado.CodigoCliente;
                    txtPNombre.Text = _clienteSeleccionado.P_Nombre;
                    txtSNombre.Text = _clienteSeleccionado.S_Nombre ?? "";
                    txtPApellido.Text = _clienteSeleccionado.P_Apellido;
                    txtSApellido.Text = _clienteSeleccionado.S_Apellido ?? "";
                    txtTelefono.Text = _clienteSeleccionado.Telefono ?? "";
                    txtEmail.Text = _clienteSeleccionado.Email ?? "";
                    cmbTipoMembresia.Text = _clienteSeleccionado.TipoMembresia ?? "";
                    chkEstado.Checked = _clienteSeleccionado.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Clientes");
                }
            }
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCodigo.Text) || 
                    string.IsNullOrWhiteSpace(txtPNombre.Text) || 
                    string.IsNullOrWhiteSpace(txtPApellido.Text))
                {
                    MessageBox.Show("El código, primer nombre y primer apellido son obligatorios.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar permisos
                if (_clienteSeleccionado == null && !PermissionService.PuedeCrear("Clientes"))
                {
                    MessageBox.Show("No tiene permisos para crear clientes.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_clienteSeleccionado != null && !PermissionService.PuedeActualizar("Clientes"))
                {
                    MessageBox.Show("No tiene permisos para actualizar clientes.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblEstado.Text = _clienteSeleccionado == null ? "Creando cliente..." : "Actualizando cliente...";
                HabilitarControles(false);

                if (_clienteSeleccionado == null)
                {
                    // Crear nuevo
                    var nuevo = new ClienteCreateDto
                    {
                        CodigoCliente = txtCodigo.Text.Trim(),
                        P_Nombre = txtPNombre.Text.Trim(),
                        S_Nombre = string.IsNullOrWhiteSpace(txtSNombre.Text) ? null : txtSNombre.Text.Trim(),
                        P_Apellido = txtPApellido.Text.Trim(),
                        S_Apellido = string.IsNullOrWhiteSpace(txtSApellido.Text) ? null : txtSApellido.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                        TipoMembresia = string.IsNullOrWhiteSpace(cmbTipoMembresia.Text) ? null : cmbTipoMembresia.Text.Trim()
                    };

                    var creado = await _clienteRepo.CreateAsync(nuevo);
                    if (creado != null)
                    {
                        MessageBox.Show("Cliente creado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarClientes();
                    }
                }
                else
                {
                    // Actualizar existente
                    var actualizado = new ClienteUpdateDto
                    {
                        CodigoCliente = txtCodigo.Text.Trim(),
                        P_Nombre = txtPNombre.Text.Trim(),
                        S_Nombre = string.IsNullOrWhiteSpace(txtSNombre.Text) ? null : txtSNombre.Text.Trim(),
                        P_Apellido = txtPApellido.Text.Trim(),
                        S_Apellido = string.IsNullOrWhiteSpace(txtSApellido.Text) ? null : txtSApellido.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                        TipoMembresia = string.IsNullOrWhiteSpace(cmbTipoMembresia.Text) ? null : cmbTipoMembresia.Text.Trim(),
                        Estado = chkEstado.Checked
                    };

                    var ok = await _clienteRepo.UpdateAsync(_clienteSeleccionado.Id_Cliente, actualizado);
                    if (ok)
                    {
                        MessageBox.Show("Cliente actualizado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarClientes();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cliente: {ex.Message}", "Error",
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
            if (_clienteSeleccionado == null)
            {
                MessageBox.Show("Seleccione un cliente para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermissionService.PuedeEliminar("Clientes"))
            {
                MessageBox.Show("No tiene permisos para eliminar clientes.", "Acceso Denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar el cliente '{_clienteSeleccionado.NombreCompleto}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    lblEstado.Text = "Eliminando cliente...";
                    HabilitarControles(false);

                    var ok = await _clienteRepo.DeleteAsync(_clienteSeleccionado.Id_Cliente);
                    if (ok)
                    {
                        MessageBox.Show("Cliente eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarClientes();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error",
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

            dgvClientes.Rows.Clear();
            var filtrados = _clientes.Where(c =>
                c.NombreCompleto.ToLower().Contains(busqueda) ||
                c.CodigoCliente.ToLower().Contains(busqueda) ||
                (c.Telefono?.ToLower().Contains(busqueda) ?? false) ||
                (c.Email?.ToLower().Contains(busqueda) ?? false)
            ).OrderBy(c => c.NombreCompleto);

            foreach (var cliente in filtrados)
            {
                dgvClientes.Rows.Add(
                    cliente.Id_Cliente,
                    cliente.CodigoCliente,
                    cliente.NombreCompleto,
                    cliente.Telefono ?? "",
                    cliente.Email ?? "",
                    cliente.TipoMembresia ?? "",
                    cliente.PuntosCompra,
                    cliente.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

