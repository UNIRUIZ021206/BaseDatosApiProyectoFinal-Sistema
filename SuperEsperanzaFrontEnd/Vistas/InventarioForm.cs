using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class InventarioForm : Form
    {
        private readonly ReporteRepository _reporteRepo;
        private List<InventarioGeneralDto> _inventario = new();

        // Paleta de colores
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);

        public InventarioForm()
        {
            InitializeComponent();
            _reporteRepo = new ReporteRepository();
            this.Shown += async (s, e) => await CargarInventario();
            
            // Aplicar estilos modernos a los DataGridViews
            DataGridViewHelper.AplicarEstiloModerno(dgvInventario);
        }

        private async Task CargarInventario()
        {
            try
            {
                // Verificar permisos antes de cargar
                if (!PermissionService.PuedeVerReporteInventario())
                {
                    MessageBox.Show("No tiene permisos para ver reportes de inventario.\n\nSolo Administradores, Bodegueros, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                lblEstado.Text = "Cargando inventario...";
                HabilitarControles(false);

                _inventario = await _reporteRepo.GetInventarioGeneralAsync();
                ActualizarGrid();

                var totalProductos = _inventario.Count;
                var productosActivos = _inventario.Count(p => p.EstadoProducto);
                var stockBajo = _inventario.Count(p => p.StockDisponibleEnLotes <= 10 && p.EstadoProducto);
                var valorTotal = _inventario.Where(p => p.EstadoProducto).Sum(p => p.PrecioVenta * p.StockDisponibleEnLotes);

                lblTotalProductos.Text = totalProductos.ToString();
                lblProductosActivos.Text = productosActivos.ToString();
                lblStockBajo.Text = stockBajo.ToString();
                lblValorTotal.Text = valorTotal.ToString("C");

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
                MessageBox.Show($"Error al cargar inventario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
                HabilitarControles(true);
            }
        }

        private void ActualizarGrid()
        {
            dgvInventario.Rows.Clear();
            foreach (var item in _inventario.OrderBy(i => i.NombreProducto))
            {
                var estado = item.EstadoProducto ? "Activo" : "Inactivo";
                var diferencia = item.StockActual - item.StockDisponibleEnLotes;
                var alertaStock = item.StockDisponibleEnLotes <= 10 && item.EstadoProducto ? "⚠️" : "";

                dgvInventario.Rows.Add(
                    item.CodigoProducto,
                    item.NombreProducto,
                    item.NombreCategoria,
                    item.PrecioVenta.ToString("C"),
                    item.StockActual,
                    item.StockDisponibleEnLotes,
                    diferencia,
                    estado,
                    alertaStock
                );
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            var busqueda = txtBuscar.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(busqueda))
            {
                ActualizarGrid();
                return;
            }

            dgvInventario.Rows.Clear();
            var filtrados = _inventario.Where(i =>
                i.CodigoProducto.ToLower().Contains(busqueda) ||
                i.NombreProducto.ToLower().Contains(busqueda) ||
                i.NombreCategoria.ToLower().Contains(busqueda)
            );

            foreach (var item in filtrados.OrderBy(i => i.NombreProducto))
            {
                var estado = item.EstadoProducto ? "Activo" : "Inactivo";
                var diferencia = item.StockActual - item.StockDisponibleEnLotes;
                var alertaStock = item.StockDisponibleEnLotes <= 10 && item.EstadoProducto ? "⚠️" : "";

                dgvInventario.Rows.Add(
                    item.CodigoProducto,
                    item.NombreProducto,
                    item.NombreCategoria,
                    item.PrecioVenta.ToString("C"),
                    item.StockActual,
                    item.StockDisponibleEnLotes,
                    diferencia,
                    estado,
                    alertaStock
                );
            }
        }

        private void HabilitarControles(bool habilitar)
        {
            txtBuscar.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
            dgvInventario.Enabled = habilitar;
            btnRefrescar.Enabled = habilitar;
        }

        private async void btnRefrescar_Click(object? sender, EventArgs e)
        {
            await CargarInventario();
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
        }
    }
}

