using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class ReporteVentasForm : Form
    {
        private readonly ReporteRepository _reporteRepo;
        private List<ReporteVentasDto> _reporte = new();

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public ReporteVentasForm()
        {
            InitializeComponent();
            _reporteRepo = new ReporteRepository();
            dtpFecha.Value = DateTime.Now;
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvReporte);
        }

        private async void btnBuscar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Verificar permisos antes de cargar
                if (!PermissionService.PuedeVerReporteVentas())
                {
                    MessageBox.Show("No tiene permisos para ver reportes de ventas.\n\nSolo Administradores, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblEstado.Text = "Cargando reporte...";
                HabilitarControles(false);

                _reporte = await _reporteRepo.GetReporteVentasAsync(dtpFecha.Value);
                ActualizarGrid();

                var totalVentas = _reporte.Sum(r => r.TotalVenta);
                var totalCantidad = _reporte.Sum(r => r.CantidadVendida);
                lblTotalVentas.Text = totalVentas.ToString("C");
                lblTotalCantidad.Text = totalCantidad.ToString();
                lblTotalProductos.Text = _reporte.Count.ToString();

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
                MessageBox.Show($"Error al cargar reporte: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGrid()
        {
            dgvReporte.Rows.Clear();
            foreach (var item in _reporte.OrderByDescending(r => r.TotalVenta))
            {
                dgvReporte.Rows.Add(
                    item.Producto,
                    item.CantidadVendida,
                    item.TotalVenta.ToString("C")
                );
            }
        }

        private void HabilitarControles(bool habilitar)
        {
            dtpFecha.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
            dgvReporte.Enabled = habilitar;
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

