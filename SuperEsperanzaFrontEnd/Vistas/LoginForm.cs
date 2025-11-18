using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Repositorio.Interfaces;
using SuperEsperanzaFrontEnd.Services;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class LoginForm : Form
    {
        private readonly IAuthRepository _authRepository;

        // Paleta de colores "Esperanza Fresca"
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);   // #2A9D8F
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);        // #264653
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106); // #E9C46A
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);         // #F4F4F4
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);          // #E76F51

        public LoginForm()
        {
            InitializeComponent();
            _authRepository = new AuthRepository();
            lblMensaje.Text = string.Empty;
            this.txtUsuario.KeyDown += TxtUsuario_KeyDown;
            this.txtContrasena.KeyDown += TxtContrasena_KeyDown;
        }

        private void TxtUsuario_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtContrasena.Focus();
            }
        }

        private void TxtContrasena_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MostrarError("Por favor ingrese su usuario");
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MostrarError("Por favor ingrese su contraseña");
                txtContrasena.Focus();
                return;
            }

            // Deshabilitar controles durante el login
            HabilitarControles(false);
            lblMensaje.Text = "Iniciando sesión...";
            lblMensaje.ForeColor = AzulOscuro;

            try
            {
                var loginRequest = new LoginRequest
                {
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contrasena = txtContrasena.Text
                };

                var loginResponse = await _authRepository.LoginAsync(loginRequest);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    // Asegurar que la fecha de expiración esté en UTC
                    var expiracion = loginResponse.Expiracion;
                    if (expiracion.Kind != DateTimeKind.Utc)
                    {
                        // Si no está en UTC, asumir que es UTC y marcarlo como tal
                        expiracion = DateTime.SpecifyKind(expiracion, DateTimeKind.Utc);
                    }

                    // Guardar el token
                    TokenService.GuardarToken(
                        loginResponse.Token,
                        loginResponse.Usuario,
                        loginResponse.Rol,
                        expiracion
                    );

                    // Verificar que el token se guardó correctamente
                    var tokenGuardado = TokenService.ObtenerToken();
                    if (string.IsNullOrEmpty(tokenGuardado))
                    {
                        var infoToken = TokenService.ObtenerInfoToken();
                        throw new Exception($"Error: El token no se guardó correctamente. Info: {infoToken}");
                    }

                    // Mostrar mensaje de éxito
                    lblMensaje.Text = $"Bienvenido, {loginResponse.Usuario} ({loginResponse.Rol})";
                    lblMensaje.ForeColor = VerdePrincipal;

                    // Cerrar el formulario de login después de un breve delay
                    await Task.Delay(500);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MostrarError("Error: No se recibió respuesta del servidor");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MostrarError(ex.Message);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }
            finally
            {
                HabilitarControles(true);
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void HabilitarControles(bool habilitar)
        {
            txtUsuario.Enabled = habilitar;
            txtContrasena.Enabled = habilitar;
            btnLogin.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = Alerta;
        }

        private void MostrarExito(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = VerdePrincipal;
        }
    }
}
