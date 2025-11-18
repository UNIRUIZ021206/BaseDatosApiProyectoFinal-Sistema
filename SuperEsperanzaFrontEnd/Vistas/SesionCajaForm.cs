using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class SesionCajaForm : Form
    {
        private readonly SesionRepository _sesionRepo;
        private List<SesionDto> _sesionesActivas = new();

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public SesionCajaForm()
        {
            InitializeComponent();
            _sesionRepo = new SesionRepository();
            AplicarPermisosIniciales();
            this.Shown += async (s, e) => await CargarSesiones();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvSesiones);
        }

        private void AplicarPermisosIniciales()
        {
            // Aplicar permisos al cargar el formulario
            if (!PermissionService.PuedeAbrirSesion())
            {
                txtCodigoSesion.Enabled = false;
                txtMontoInicial.Enabled = false;
                txtObservacion.Enabled = false;
                btnAbrirSesion.Enabled = false;
                btnAbrirSesion.BackColor = System.Drawing.Color.Gray;
            }

            if (!PermissionService.PuedeCerrarSesion())
            {
                btnCerrarSesion.Enabled = false;
                btnCerrarSesion.BackColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Genera un código de sesión único que incluye timestamp con milisegundos y un identificador único
        /// </summary>
        private string GenerarCodigoSesionUnico()
        {
            var random = new Random();
            var now = DateTime.Now;
            
            // Formato de 10 caracteres: SES + DDMM + HH + RR
            // SES (3) + DDMM (4) + HH (2) + RR (2) = 11 caracteres... no cabe
            // Formato alternativo de 10 caracteres: SES + DDMM + HH + R
            // SES (3) + DDMM (4) + HH (2) + R (1) = 10 caracteres exactos
            // DDMM = día y mes (2 dígitos cada uno)
            // HH = hora (2 dígitos)
            // R = 1 dígito aleatorio (0-9) para evitar colisiones en la misma hora
            
            var diaMes = $"{now.Day:00}{now.Month:00}"; // DDMM (4 caracteres)
            var hora = $"{now.Hour:00}"; // HH (2 caracteres)
            var aleatorio = random.Next(0, 10); // 1 dígito aleatorio (0-9)
            
            // Formato final: SES + DDMM + HH + R = 10 caracteres
            var codigo = $"SES{diaMes}{hora}{aleatorio}";
            
            // Verificar si el código ya existe en las sesiones activas
            // Si existe, regenerar con un dígito aleatorio diferente
            int intentos = 0;
            while (_sesionesActivas.Any(s => s.CodigoSesion.Equals(codigo, StringComparison.OrdinalIgnoreCase)) && intentos < 10)
            {
                aleatorio = random.Next(0, 10);
                codigo = $"SES{diaMes}{hora}{aleatorio}";
                intentos++;
            }
            
            // Si después de 10 intentos sigue duplicado, agregar segundo del timestamp
            if (intentos >= 10)
            {
                var segundo = now.Second % 10; // Último dígito del segundo
                codigo = $"SES{diaMes}{hora}{segundo}";
            }
            
            return codigo;
        }

        private async Task CargarSesiones()
        {
            try
            {
                lblEstado.Text = "Cargando sesiones...";
                HabilitarControles(false);

                _sesionesActivas = await _sesionRepo.GetActivasAsync();
                ActualizarGridSesiones();

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
                MessageBox.Show($"Error al cargar sesiones: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGridSesiones()
        {
            dgvSesiones.Rows.Clear();
            foreach (var sesion in _sesionesActivas)
            {
                dgvSesiones.Rows.Add(
                    sesion.Id_Sesion,
                    sesion.CodigoSesion ?? string.Empty,
                    sesion.UsuarioNombre ?? string.Empty,
                    sesion.FechaApertura.ToString("dd/MM/yyyy HH:mm"),
                    sesion.MontoInicial.ToString("C"),
                    sesion.Estado ? "Activa" : "Cerrada"
                );
            }
        }

        private async void btnAbrirSesion_Click(object? sender, EventArgs e)
        {
            // Verificar permisos
            if (!PermissionService.PuedeAbrirSesion())
            {
                MessageBox.Show("No tiene permisos para abrir sesiones de caja.\n\nSolo Administradores y Cajeros pueden abrir sesiones.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigoSesion = string.Empty;
            try
            {
                // Generar código automático si no se ingresó uno
                if (string.IsNullOrWhiteSpace(txtCodigoSesion.Text))
                {
                    codigoSesion = GenerarCodigoSesionUnico();
                    txtCodigoSesion.Text = codigoSesion;
                }
                else
                {
                    codigoSesion = txtCodigoSesion.Text.Trim();
                    
                    // Verificar si el código ya existe en las sesiones activas
                    if (_sesionesActivas.Any(s => s.CodigoSesion.Equals(codigoSesion, StringComparison.OrdinalIgnoreCase)))
                    {
                        var resultado = MessageBox.Show(
                            $"El código de sesión '{codigoSesion}' ya existe.\n\n¿Desea generar uno automático?",
                            "Código Duplicado",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        
                        if (resultado == DialogResult.Yes)
                        {
                            codigoSesion = GenerarCodigoSesionUnico();
                            txtCodigoSesion.Text = codigoSesion;
                        }
                        else
                        {
                            return; // El usuario canceló
                        }
                    }
                }

                if (!decimal.TryParse(txtMontoInicial.Text, out var montoInicial) || montoInicial < 0)
                {
                    MessageBox.Show("Por favor ingrese un monto inicial válido.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMontoInicial.Focus();
                    return;
                }

                lblEstado.Text = "Abriendo sesión...";
                HabilitarControles(false);

                var sesionCreate = new SesionCreateDto
                {
                    CodigoSesion = codigoSesion,
                    MontoInicial = montoInicial,
                    Observacion = txtObservacion.Text.Trim()
                };

                var sesionCreada = await _sesionRepo.AbrirAsync(sesionCreate);

                if (sesionCreada != null)
                {
                    MessageBox.Show(
                        $"Sesión abierta exitosamente.\nCódigo: {sesionCreada.CodigoSesion}\nMonto Inicial: {sesionCreada.MontoInicial:C}",
                        "Sesión Abierta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Limpiar campos
                    txtCodigoSesion.Clear();
                    txtMontoInicial.Text = "0";
                    txtObservacion.Clear();

                    // Recargar sesiones
                    await CargarSesiones();
                    
                    // Si se abrió desde PuntoVentaForm, cerrar este formulario
                    if (this.Modal)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
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
                // Verificar si el error es por código duplicado
                var mensajeError = ex.Message;
                if (mensajeError.Contains("UNIQUE KEY constraint") || mensajeError.Contains("duplicate key") || mensajeError.Contains("duplicado"))
                {
                    var resultado = MessageBox.Show(
                        $"El código de sesión '{codigoSesion}' ya existe en la base de datos.\n\n¿Desea generar un código automático nuevo?",
                        "Código Duplicado",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (resultado == DialogResult.Yes)
                    {
                        // Generar nuevo código automático
                        var nuevoCodigo = GenerarCodigoSesionUnico();
                        txtCodigoSesion.Text = nuevoCodigo;
                        txtCodigoSesion.Focus();
                        lblEstado.Text = "Código regenerado. Puede modificarlo o intentar abrir la sesión nuevamente.";
                    }
                    else
                    {
                        lblEstado.Text = "Listo";
                    }
                }
                else
                {
                    MessageBox.Show($"Error al abrir sesión: {mensajeError}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblEstado.Text = "Listo";
                }
                HabilitarControles(true);
            }
        }

        private async void btnCerrarSesion_Click(object? sender, EventArgs e)
        {
            // Verificar permisos
            if (!PermissionService.PuedeCerrarSesion())
            {
                MessageBox.Show("No tiene permisos para cerrar sesiones de caja.\n\nSolo Administradores, Cajeros y Supervisores pueden cerrar sesiones.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSesiones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una sesión para cerrar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var idSesion = Convert.ToInt32(dgvSesiones.SelectedRows[0].Cells[0].Value);
                var codigoSesion = dgvSesiones.SelectedRows[0].Cells[1].Value?.ToString() ?? "";

                var resultado = MessageBox.Show(
                    $"¿Está seguro que desea cerrar la sesión {codigoSesion}?",
                    "Cerrar Sesión",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes) return;

                lblEstado.Text = "Cerrando sesión...";
                HabilitarControles(false);

                var cerrado = await _sesionRepo.CerrarAsync(idSesion);

                if (cerrado)
                {
                    MessageBox.Show("Sesión cerrada exitosamente.", "Sesión Cerrada",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarSesiones();
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
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Listo";
                HabilitarControles(true);
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigoSesion.Enabled = habilitar && PermissionService.PuedeAbrirSesion();
            txtMontoInicial.Enabled = habilitar && PermissionService.PuedeAbrirSesion();
            txtObservacion.Enabled = habilitar && PermissionService.PuedeAbrirSesion();
            btnAbrirSesion.Enabled = habilitar && PermissionService.PuedeAbrirSesion();
            btnCerrarSesion.Enabled = habilitar && PermissionService.PuedeCerrarSesion();
            dgvSesiones.Enabled = habilitar;
        }
    }
}

