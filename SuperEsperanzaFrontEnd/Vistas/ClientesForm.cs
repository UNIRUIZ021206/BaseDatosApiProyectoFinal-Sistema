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
        private ErrorProvider errorProvider = new ErrorProvider();

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
            
            // Configurar validaciones en tiempo real
            ConfigurarValidaciones();
        }
        
        private void ConfigurarValidaciones()
        {
            // Validar que nombres y apellidos no acepten números
            txtPNombre.KeyPress += TxtNombre_KeyPress;
            txtSNombre.KeyPress += TxtNombre_KeyPress;
            txtPApellido.KeyPress += TxtNombre_KeyPress;
            txtSApellido.KeyPress += TxtNombre_KeyPress;
            
            txtPNombre.Validating += TxtPNombre_Validating;
            txtSNombre.Validating += TxtSNombre_Validating;
            txtPApellido.Validating += TxtPApellido_Validating;
            txtSApellido.Validating += TxtSApellido_Validating;
            
            // Validar teléfono solo números
            txtTelefono.KeyPress += TxtTelefono_KeyPress;
            txtTelefono.Validating += TxtTelefono_Validating;
            
            // Validar email
            txtEmail.Validating += TxtEmail_Validating;
            
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
                errorProvider.SetError((Control)sender!, "Los nombres no pueden contener números.");
                return;
            }
            
            // Permitir letras, espacios, guiones y apóstrofes
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
            {
                e.Handled = true;
                errorProvider.SetError((Control)sender!, "Solo se permiten letras, espacios, guiones y apóstrofes.");
            }
            else
            {
                errorProvider.SetError((Control)sender!, "");
            }
        }
        
        private void TxtPNombre_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPNombre.Text))
            {
                errorProvider.SetError(txtPNombre, "El primer nombre es obligatorio.");
                e.Cancel = true;
            }
            else if (txtPNombre.Text.Trim().Length > 50)
            {
                errorProvider.SetError(txtPNombre, "El primer nombre no puede exceder 50 caracteres.");
                e.Cancel = true;
            }
            else if (txtPNombre.Text.Any(char.IsDigit))
            {
                errorProvider.SetError(txtPNombre, "El primer nombre no puede contener números.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtPNombre, "");
            }
        }
        
        private void TxtSNombre_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSNombre.Text))
            {
                if (txtSNombre.Text.Trim().Length > 50)
                {
                    errorProvider.SetError(txtSNombre, "El segundo nombre no puede exceder 50 caracteres.");
                    e.Cancel = true;
                }
                else if (txtSNombre.Text.Any(char.IsDigit))
                {
                    errorProvider.SetError(txtSNombre, "El segundo nombre no puede contener números.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError(txtSNombre, "");
                }
            }
            else
            {
                errorProvider.SetError(txtSNombre, "");
            }
        }
        
        private void TxtPApellido_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPApellido.Text))
            {
                errorProvider.SetError(txtPApellido, "El primer apellido es obligatorio.");
                e.Cancel = true;
            }
            else if (txtPApellido.Text.Trim().Length > 50)
            {
                errorProvider.SetError(txtPApellido, "El primer apellido no puede exceder 50 caracteres.");
                e.Cancel = true;
            }
            else if (txtPApellido.Text.Any(char.IsDigit))
            {
                errorProvider.SetError(txtPApellido, "El primer apellido no puede contener números.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtPApellido, "");
            }
        }
        
        private void TxtSApellido_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSApellido.Text))
            {
                if (txtSApellido.Text.Trim().Length > 50)
                {
                    errorProvider.SetError(txtSApellido, "El segundo apellido no puede exceder 50 caracteres.");
                    e.Cancel = true;
                }
                else if (txtSApellido.Text.Any(char.IsDigit))
                {
                    errorProvider.SetError(txtSApellido, "El segundo apellido no puede contener números.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError(txtSApellido, "");
                }
            }
            else
            {
                errorProvider.SetError(txtSApellido, "");
            }
        }
        
        private void TxtTelefono_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Permitir teclas de control
            if (char.IsControl(e.KeyChar))
                return;
            
            // Permitir solo números, espacios, guiones y paréntesis
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '(' && e.KeyChar != ')')
            {
                e.Handled = true;
                errorProvider.SetError(txtTelefono, "El teléfono solo puede contener números, espacios, guiones y paréntesis.");
            }
            else
            {
                errorProvider.SetError(txtTelefono, "");
            }
        }
        
        private void TxtTelefono_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                var telefonoLimpio = txtTelefono.Text.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                if (!telefonoLimpio.All(char.IsDigit))
                {
                    errorProvider.SetError(txtTelefono, "El teléfono solo puede contener números, espacios, guiones y paréntesis.");
                    e.Cancel = true;
                }
                else if (telefonoLimpio.Length < 8 || telefonoLimpio.Length > 15)
                {
                    errorProvider.SetError(txtTelefono, "El teléfono debe contener entre 8 y 15 dígitos.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError(txtTelefono, "");
                }
            }
            else
            {
                errorProvider.SetError(txtTelefono, "");
            }
        }
        
        private void TxtEmail_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (txtEmail.Text.Trim().Length > 100)
                {
                    errorProvider.SetError(txtEmail, "El email no puede exceder 100 caracteres.");
                    e.Cancel = true;
                }
                else if (!ValidarEmail(txtEmail.Text))
                {
                    errorProvider.SetError(txtEmail, "El formato del email no es válido.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError(txtEmail, "");
                }
            }
            else
            {
                errorProvider.SetError(txtEmail, "");
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
        
        private string GenerarCodigoCliente()
        {
            var random = new Random(Guid.NewGuid().GetHashCode() ^ Environment.TickCount);
            const string caracteres = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var codigo = new System.Text.StringBuilder(10);
            codigo.Append("CLI"); // Prefijo para clientes
            
            for (int i = 0; i < 7; i++)
            {
                codigo.Append(caracteres[random.Next(caracteres.Length)]);
            }
            
            return codigo.ToString();
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
            // Generar código automáticamente solo al crear nuevo
            txtCodigo.Text = GenerarCodigoCliente();
            txtCodigo.Enabled = false; // Deshabilitar edición del código
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
            errorProvider.Clear();
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
                    txtCodigo.Enabled = true; // Permitir edición al actualizar
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
                    errorProvider.Clear();
                }
            }
        }

        private bool ValidarEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // Email es opcional

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidarTelefono(string? telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return true; // Teléfono es opcional

            // Validar que solo contenga números, espacios, guiones y paréntesis
            var telefonoLimpio = telefono.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            return telefonoLimpio.All(char.IsDigit) && telefonoLimpio.Length >= 8 && telefonoLimpio.Length <= 15;
        }

        private bool ValidarSoloLetras(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return true;

            // Permitir letras, espacios y algunos caracteres especiales comunes en nombres
            return texto.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-' || c == '\'');
        }

        private bool ValidarCampos()
        {
            // Validar código
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código del cliente es obligatorio.", "Validación",
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

            // Validar primer nombre
            if (string.IsNullOrWhiteSpace(txtPNombre.Text))
            {
                MessageBox.Show("El primer nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPNombre.Focus();
                return false;
            }

            if (txtPNombre.Text.Trim().Length > 50)
            {
                MessageBox.Show("El primer nombre no puede exceder 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPNombre.Focus();
                return false;
            }

            if (!ValidarSoloLetras(txtPNombre.Text))
            {
                MessageBox.Show("El primer nombre solo puede contener letras, espacios y guiones.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPNombre.Focus();
                return false;
            }

            // Validar segundo nombre (opcional)
            if (!string.IsNullOrWhiteSpace(txtSNombre.Text))
            {
                if (txtSNombre.Text.Trim().Length > 50)
                {
                    MessageBox.Show("El segundo nombre no puede exceder 50 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSNombre.Focus();
                    return false;
                }

                if (!ValidarSoloLetras(txtSNombre.Text))
                {
                    MessageBox.Show("El segundo nombre solo puede contener letras, espacios y guiones.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSNombre.Focus();
                    return false;
                }
            }

            // Validar primer apellido
            if (string.IsNullOrWhiteSpace(txtPApellido.Text))
            {
                MessageBox.Show("El primer apellido es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPApellido.Focus();
                return false;
            }

            if (txtPApellido.Text.Trim().Length > 50)
            {
                MessageBox.Show("El primer apellido no puede exceder 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPApellido.Focus();
                return false;
            }

            if (!ValidarSoloLetras(txtPApellido.Text))
            {
                MessageBox.Show("El primer apellido solo puede contener letras, espacios y guiones.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPApellido.Focus();
                return false;
            }

            // Validar segundo apellido (opcional)
            if (!string.IsNullOrWhiteSpace(txtSApellido.Text))
            {
                if (txtSApellido.Text.Trim().Length > 50)
                {
                    MessageBox.Show("El segundo apellido no puede exceder 50 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSApellido.Focus();
                    return false;
                }

                if (!ValidarSoloLetras(txtSApellido.Text))
                {
                    MessageBox.Show("El segundo apellido solo puede contener letras, espacios y guiones.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSApellido.Focus();
                    return false;
                }
            }

            // Validar teléfono
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                if (!ValidarTelefono(txtTelefono.Text))
                {
                    MessageBox.Show("El teléfono debe contener entre 8 y 15 dígitos. Puede incluir espacios, guiones y paréntesis.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTelefono.Focus();
                    return false;
                }
            }

            // Validar email
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (txtEmail.Text.Trim().Length > 100)
                {
                    MessageBox.Show("El email no puede exceder 100 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }

                if (!ValidarEmail(txtEmail.Text))
                {
                    MessageBox.Show("El formato del email no es válido.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Validar dirección (opcional)
            if (!string.IsNullOrWhiteSpace(txtDireccion.Text) && txtDireccion.Text.Trim().Length > 200)
            {
                MessageBox.Show("La dirección no puede exceder 200 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDireccion.Focus();
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

