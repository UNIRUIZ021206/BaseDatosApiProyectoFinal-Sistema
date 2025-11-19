using SuperEsperanzaFrontEnd.Modelos.Dto;
using SuperEsperanzaFrontEnd.Repositorio;
using SuperEsperanzaFrontEnd.Services;
using SuperEsperanzaFrontEnd.Helpers;
using ClosedXML.Excel;
using System.IO;
using ScottPlot;
using ScottPlot.Plottables;

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
            
            // Inicializar gráficas
            InicializarGraficas();
        }
        
        private void InicializarGraficas()
        {
            // Configurar gráfica de barras de ventas
            formsPlotVentas.Plot.Title("Ventas por Producto");
            formsPlotVentas.Plot.Axes.Bottom.Label.Text = "Productos";
            formsPlotVentas.Plot.Axes.Left.Label.Text = "Total Ventas ($)";
            formsPlotVentas.Plot.ShowGrid();
            formsPlotVentas.Refresh();
            
            // Configurar gráfica de cantidades
            formsPlotCantidades.Plot.Title("Cantidades Vendidas");
            formsPlotCantidades.Plot.Axes.Bottom.Label.Text = "Productos";
            formsPlotCantidades.Plot.Axes.Left.Label.Text = "Cantidad Vendida";
            formsPlotCantidades.Plot.ShowGrid();
            formsPlotCantidades.Refresh();
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
                ActualizarGraficas();

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

        private void ActualizarGraficas()
        {
            if (!_reporte.Any())
            {
                formsPlotVentas.Plot.Clear();
                formsPlotCantidades.Plot.Clear();
                formsPlotVentas.Refresh();
                formsPlotCantidades.Refresh();
                return;
            }

            // Ordenar por total de ventas (top 10 para mejor visualización)
            var topProductos = _reporte.OrderByDescending(r => r.TotalVenta).Take(10).ToList();
            
            if (!topProductos.Any())
            {
                formsPlotVentas.Plot.Clear();
                formsPlotCantidades.Plot.Clear();
                formsPlotVentas.Refresh();
                formsPlotCantidades.Refresh();
                return;
            }

            // Preparar datos para gráfica de ventas
            var productos = topProductos.Select(r => r.Producto.Length > 20 ? r.Producto.Substring(0, 20) + "..." : r.Producto).ToArray();
            var ventas = topProductos.Select(r => (double)r.TotalVenta).ToArray();
            var posiciones = Enumerable.Range(0, productos.Length).Select(i => (double)i).ToArray();

            // Calcular límites del eje Y para ventas (empezar en 0, máximo con margen del 15%)
            var maxVentas = ventas.Length > 0 ? ventas.Max() : 100;
            var minVentas = 0;
            var maxVentasConMargen = maxVentas * 1.15; // Agregar 15% de margen superior

            // Limpiar y actualizar gráfica de ventas
            formsPlotVentas.Plot.Clear();
            var barVentas = formsPlotVentas.Plot.Add.Bars(posiciones, ventas);
            barVentas.Color = new ScottPlot.Color(VerdePrincipal.R, VerdePrincipal.G, VerdePrincipal.B);
            
            // Configurar etiquetas del eje X usando la API correcta de ScottPlot 5.0
            var tickGeneratorVentas = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < posiciones.Length; i++)
            {
                tickGeneratorVentas.AddMajor(posiciones[i], productos[i]);
            }
            formsPlotVentas.Plot.Axes.Bottom.TickGenerator = tickGeneratorVentas;
            formsPlotVentas.Plot.Axes.Bottom.TickLabelStyle.Rotation = 45;
            formsPlotVentas.Plot.Axes.Bottom.TickLabelStyle.FontSize = 8;
            
            // Configurar eje Y: empezar en 0, sin valores negativos
            formsPlotVentas.Plot.Axes.Left.Range.Set(minVentas, maxVentasConMargen);
            
            formsPlotVentas.Plot.Title($"Top 10 Productos - Ventas del {dtpFecha.Value:dd/MM/yyyy}");
            formsPlotVentas.Plot.Axes.Bottom.Label.Text = "Productos";
            formsPlotVentas.Plot.Axes.Left.Label.Text = "Total Ventas ($)";
            formsPlotVentas.Plot.ShowGrid();
            formsPlotVentas.Refresh();
            Application.DoEvents();

            // Preparar datos para gráfica de cantidades
            var cantidades = topProductos.Select(r => (double)r.CantidadVendida).ToArray();

            // Calcular límites del eje Y para cantidades (empezar en 0, máximo con margen del 15%)
            var maxCantidades = cantidades.Length > 0 ? cantidades.Max() : 100;
            var minCantidades = 0;
            var maxCantidadesConMargen = maxCantidades * 1.15; // Agregar 15% de margen superior

            // Limpiar y actualizar gráfica de cantidades
            formsPlotCantidades.Plot.Clear();
            var barCantidades = formsPlotCantidades.Plot.Add.Bars(posiciones, cantidades);
            barCantidades.Color = new ScottPlot.Color(AcentoBrillante.R, AcentoBrillante.G, AcentoBrillante.B);
            
            // Configurar etiquetas del eje X
            var tickGeneratorCantidades = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < posiciones.Length; i++)
            {
                tickGeneratorCantidades.AddMajor(posiciones[i], productos[i]);
            }
            formsPlotCantidades.Plot.Axes.Bottom.TickGenerator = tickGeneratorCantidades;
            formsPlotCantidades.Plot.Axes.Bottom.TickLabelStyle.Rotation = 45;
            formsPlotCantidades.Plot.Axes.Bottom.TickLabelStyle.FontSize = 8;
            
            // Configurar eje Y: empezar en 0, sin valores negativos
            formsPlotCantidades.Plot.Axes.Left.Range.Set(minCantidades, maxCantidadesConMargen);
            
            formsPlotCantidades.Plot.Title($"Top 10 Productos - Cantidades del {dtpFecha.Value:dd/MM/yyyy}");
            formsPlotCantidades.Plot.Axes.Bottom.Label.Text = "Productos";
            formsPlotCantidades.Plot.Axes.Left.Label.Text = "Cantidad Vendida";
            formsPlotCantidades.Plot.ShowGrid();
            formsPlotCantidades.Refresh();
            Application.DoEvents();
        }

        private void HabilitarControles(bool habilitar)
        {
            dtpFecha.Enabled = habilitar;
            btnBuscar.Enabled = habilitar;
            btnExportarExcel.Enabled = habilitar && _reporte.Any();
            dgvReporte.Enabled = habilitar;
        }

        private void btnExportarExcel_Click(object? sender, EventArgs e)
        {
            if (!_reporte.Any())
            {
                MessageBox.Show("No hay datos para exportar. Por favor, busque un reporte primero.", 
                    "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Verificar permisos antes de exportar
            if (!PermissionService.PuedeVerReporteVentas())
            {
                MessageBox.Show("No tiene permisos para exportar reportes de ventas.\n\nSolo Administradores, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "Archivos Excel (*.xlsx)|*.xlsx",
                    FileName = $"Reporte_Ventas_{dtpFecha.Value:yyyyMMdd}.xlsx",
                    Title = "Guardar Reporte de Ventas"
                };

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                lblEstado.Text = "Exportando a Excel...";
                Application.DoEvents();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Reporte de Ventas");

                // Título
                worksheet.Cell(1, 1).Value = "Reporte de Ventas";
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = $"Fecha: {dtpFecha.Value:dd/MM/yyyy}";
                worksheet.Cell(2, 1).Style.Font.FontSize = 12;

                // Encabezados
                worksheet.Cell(4, 1).Value = "Producto";
                worksheet.Cell(4, 2).Value = "Cantidad Vendida";
                worksheet.Cell(4, 3).Value = "Total Venta";
                
                var headerRange = worksheet.Range(4, 1, 4, 3);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(38, 70, 83);
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Datos
                int fila = 5;
                foreach (var item in _reporte.OrderByDescending(r => r.TotalVenta))
                {
                    worksheet.Cell(fila, 1).Value = item.Producto;
                    worksheet.Cell(fila, 2).Value = item.CantidadVendida;
                    worksheet.Cell(fila, 3).Value = item.TotalVenta;
                    worksheet.Cell(fila, 3).Style.NumberFormat.Format = "$#,##0.00";
                    fila++;
                }

                // Totales
                fila++;
                worksheet.Cell(fila, 1).Value = "TOTALES";
                worksheet.Cell(fila, 1).Style.Font.Bold = true;
                worksheet.Cell(fila, 2).Value = _reporte.Sum(r => r.CantidadVendida);
                worksheet.Cell(fila, 2).Style.Font.Bold = true;
                worksheet.Cell(fila, 3).Value = _reporte.Sum(r => r.TotalVenta);
                worksheet.Cell(fila, 3).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(fila, 3).Style.Font.Bold = true;
                
                var totalRange = worksheet.Range(fila, 1, fila, 3);
                totalRange.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 196, 106);

                // Ajustar ancho de columnas automáticamente
                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 18;
                
                // Aplicar bordes a los datos
                var dataRange = worksheet.Range(4, 1, fila, 3);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Guardar archivo
                workbook.SaveAs(saveDialog.FileName);

                MessageBox.Show($"Reporte exportado exitosamente a:\n{saveDialog.FileName}", 
                    "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblEstado.Text = "Listo";
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message + "\n\nSerá redirigido al login.", "Sesión Expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error al guardar el archivo. Asegúrese de que el archivo no esté abierto en otro programa.\n\nDetalle: {ex.Message}", 
                    "Error de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a Excel: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error";
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

