using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class UsuariosForm : Form
    {
        private readonly UsuarioRepository _usuarioRepo;
        private readonly RolRepository _rolRepo;
        private List<UsuarioDto> _usuarios = new();
        private List<RolDto> _roles = new();
        private UsuarioDto? _usuarioSeleccionado;

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public UsuariosForm()
        {
            InitializeComponent();
            _usuarioRepo = new UsuarioRepository();
            _rolRepo = new RolRepository();
            this.Shown += async (s, e) => await CargarDatos();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvUsuarios);
        }

        private async Task CargarDatos()
        {
            try
            {
                lblEstado.Text = "Cargando datos...";
                HabilitarControles(false);

                var usuariosTask = _usuarioRepo.GetAllAsync();
                var rolesTask = _rolRepo.GetAllAsync();

                await Task.WhenAll(usuariosTask, rolesTask);

                _usuarios = await usuariosTask;
                _roles = await rolesTask;

                CargarRoles();
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

        private void CargarRoles()
        {
            cmbRol.Items.Clear();
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "Id";
            
            foreach (var rol in _roles.OrderBy(r => r.NombreRol))
            {
                cmbRol.Items.Add(rol);
            }
        }

        private void ActualizarGrid()
        {
            dgvUsuarios.Rows.Clear();
            foreach (var usuario in _usuarios.OrderBy(u => u.Nombre))
            {
                dgvUsuarios.Rows.Add(
                    usuario.Id_Usuario,
                    usuario.CodigoUsuario,
                    $"{usuario.Nombre} {usuario.Apellido}",
                    usuario.Email ?? "",
                    usuario.Telefono ?? "",
                    usuario.NombreRol ?? "",
                    usuario.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void LimpiarFormulario()
        {
            _usuarioSeleccionado = null;
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtClave.Text = "";
            cmbRol.SelectedIndex = -1;
            chkEstado.Checked = true;
            btnGuardar.Text = "Agregar";
            btnEliminar.Enabled = false;
            txtClave.Enabled = true;
            lblClave.Enabled = true;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtApellido.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtClave.Enabled = habilitar && _usuarioSeleccionado == null;
            lblClave.Enabled = habilitar && _usuarioSeleccionado == null;
            cmbRol.Enabled = habilitar;
            chkEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            dgvUsuarios.Enabled = habilitar;
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
        }

        private void dgvUsuarios_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value);
                _usuarioSeleccionado = _usuarios.FirstOrDefault(u => u.Id_Usuario == id);

                if (_usuarioSeleccionado != null)
                {
                    txtCodigo.Text = _usuarioSeleccionado.CodigoUsuario;
                    txtNombre.Text = _usuarioSeleccionado.Nombre;
                    txtApellido.Text = _usuarioSeleccionado.Apellido;
                    txtEmail.Text = _usuarioSeleccionado.Email ?? "";
                    txtTelefono.Text = _usuarioSeleccionado.Telefono ?? "";
                    txtClave.Text = "";
                    txtClave.Enabled = false;
                    lblClave.Enabled = false;
                    
                    // Seleccionar rol
                    for (int i = 0; i < cmbRol.Items.Count; i++)
                    {
                        if (cmbRol.Items[i] is RolDto rol && rol.Id == _usuarioSeleccionado.Id_Rol)
                        {
                            cmbRol.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    chkEstado.Checked = _usuarioSeleccionado.Estado;
                    btnGuardar.Text = "Actualizar";
                    btnEliminar.Enabled = PermissionService.PuedeEliminar("Usuarios");
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
                MessageBox.Show("El código del usuario es obligatorio.", "Validación",
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
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Trim().Length > 50)
            {
                MessageBox.Show("El nombre no puede exceder 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (!ValidarSoloLetras(txtNombre.Text))
            {
                MessageBox.Show("El nombre solo puede contener letras, espacios y guiones.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (txtApellido.Text.Trim().Length > 50)
            {
                MessageBox.Show("El apellido no puede exceder 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (!ValidarSoloLetras(txtApellido.Text))
            {
                MessageBox.Show("El apellido solo puede contener letras, espacios y guiones.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
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

            // Validar contraseña (solo al crear)
            if (_usuarioSeleccionado == null)
            {
                if (string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    MessageBox.Show("La contraseña es obligatoria al crear un usuario.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return false;
                }

                if (txtClave.Text.Length < 6)
                {
                    MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return false;
                }

                if (txtClave.Text.Length > 100)
                {
                    MessageBox.Show("La contraseña no puede exceder 100 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return false;
                }
            }
            else
            {
                // Al actualizar, si se proporciona contraseña, validar longitud
                if (!string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    if (txtClave.Text.Length < 6)
                    {
                        MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClave.Focus();
                        return false;
                    }

                    if (txtClave.Text.Length > 100)
                    {
                        MessageBox.Show("La contraseña no puede exceder 100 caracteres.", "Validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClave.Focus();
                        return false;
                    }
                }
            }

            // Validar rol
            if (cmbRol.SelectedIndex < 0 || cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un rol.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRol.Focus();
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
                if (_usuarioSeleccionado == null && !PermissionService.PuedeCrear("Usuarios"))
                {
                    MessageBox.Show("No tiene permisos para crear usuarios.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_usuarioSeleccionado != null && !PermissionService.PuedeActualizar("Usuarios"))
                {
                    MessageBox.Show("No tiene permisos para actualizar usuarios.", "Acceso Denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rolSeleccionado = cmbRol.SelectedItem as RolDto;
                if (rolSeleccionado == null)
                {
                    MessageBox.Show("Rol inválido.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblEstado.Text = _usuarioSeleccionado == null ? "Creando usuario..." : "Actualizando usuario...";
                HabilitarControles(false);

                if (_usuarioSeleccionado == null)
                {
                    // Crear nuevo
                    var nuevo = new UsuarioCreateDto
                    {
                        CodigoUsuario = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Clave = txtClave.Text,
                        Id_Rol = rolSeleccionado.Id
                    };

                    var creado = await _usuarioRepo.CreateAsync(nuevo);
                    if (creado != null)
                    {
                        MessageBox.Show("Usuario creado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
                else
                {
                    // Actualizar existente
                    var actualizado = new UsuarioUpdateDto
                    {
                        CodigoUsuario = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                        Clave = string.IsNullOrWhiteSpace(txtClave.Text) ? null : txtClave.Text,
                        Id_Rol = rolSeleccionado.Id,
                        Estado = chkEstado.Checked
                    };

                    var ok = await _usuarioRepo.UpdateAsync(_usuarioSeleccionado.Id_Usuario, actualizado);
                    if (ok)
                    {
                        MessageBox.Show("Usuario actualizado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error",
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
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermissionService.PuedeEliminar("Usuarios"))
            {
                MessageBox.Show("No tiene permisos para eliminar usuarios.", "Acceso Denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar el usuario '{_usuarioSeleccionado.Nombre} {_usuarioSeleccionado.Apellido}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    lblEstado.Text = "Eliminando usuario...";
                    HabilitarControles(false);

                    var ok = await _usuarioRepo.DeleteAsync(_usuarioSeleccionado.Id_Usuario);
                    if (ok)
                    {
                        MessageBox.Show("Usuario eliminado exitosamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
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

            dgvUsuarios.Rows.Clear();
            var filtrados = _usuarios.Where(u =>
                u.Nombre.ToLower().Contains(busqueda) ||
                u.Apellido.ToLower().Contains(busqueda) ||
                u.CodigoUsuario.ToLower().Contains(busqueda) ||
                (u.Email?.ToLower().Contains(busqueda) ?? false) ||
                (u.NombreRol?.ToLower().Contains(busqueda) ?? false)
            ).OrderBy(u => u.Nombre);

            foreach (var usuario in filtrados)
            {
                dgvUsuarios.Rows.Add(
                    usuario.Id_Usuario,
                    usuario.CodigoUsuario,
                    $"{usuario.Nombre} {usuario.Apellido}",
                    usuario.Email ?? "",
                    usuario.Telefono ?? "",
                    usuario.NombreRol ?? "",
                    usuario.Estado ? "Activo" : "Inactivo"
                );
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

