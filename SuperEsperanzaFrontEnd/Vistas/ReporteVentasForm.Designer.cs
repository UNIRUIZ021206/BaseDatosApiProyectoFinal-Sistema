#nullable enable
using ScottPlot.WinForms;

namespace SuperEsperanzaFrontEnd.Vistas
{
    partial class ReporteVentasForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSuperior = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelControles = new System.Windows.Forms.Panel();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.panelResumen = new System.Windows.Forms.Panel();
            this.lblTotalProductosLabel = new System.Windows.Forms.Label();
            this.lblTotalProductos = new System.Windows.Forms.Label();
            this.lblTotalCantidadLabel = new System.Windows.Forms.Label();
            this.lblTotalCantidad = new System.Windows.Forms.Label();
            this.lblTotalVentasLabel = new System.Windows.Forms.Label();
            this.lblTotalVentas = new System.Windows.Forms.Label();
            this.panelGraficas = new System.Windows.Forms.Panel();
            this.formsPlotCantidades = new ScottPlot.WinForms.FormsPlot();
            this.formsPlotVentas = new ScottPlot.WinForms.FormsPlot();
            this.panelTabla = new System.Windows.Forms.Panel();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.panelSuperior.SuspendLayout();
            this.panelControles.SuspendLayout();
            this.panelResumen.SuspendLayout();
            this.panelGraficas.SuspendLayout();
            this.panelTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.panelSuperior.Controls.Add(this.lblTitulo);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1400, 60);
            this.panelSuperior.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reporte de Ventas";
            // 
            // panelControles
            // 
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelControles.Controls.Add(this.btnExportarExcel);
            this.panelControles.Controls.Add(this.btnBuscar);
            this.panelControles.Controls.Add(this.dtpFecha);
            this.panelControles.Controls.Add(this.lblFecha);
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControles.Location = new System.Drawing.Point(0, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(1400, 60);
            this.panelControles.TabIndex = 1;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblFecha.Location = new System.Drawing.Point(20, 20);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(48, 19);
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpFecha.Location = new System.Drawing.Point(80, 18);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(250, 25);
            this.dtpFecha.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(350, 18);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 30);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExportarExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportarExcel.Location = new System.Drawing.Point(490, 18);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(150, 30);
            this.btnExportarExcel.TabIndex = 3;
            this.btnExportarExcel.Text = "Exportar a Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Enabled = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // panelResumen
            // 
            this.panelResumen.BackColor = System.Drawing.Color.White;
            this.panelResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelResumen.Controls.Add(this.lblTotalProductos);
            this.panelResumen.Controls.Add(this.lblTotalProductosLabel);
            this.panelResumen.Controls.Add(this.lblTotalCantidad);
            this.panelResumen.Controls.Add(this.lblTotalCantidadLabel);
            this.panelResumen.Controls.Add(this.lblTotalVentas);
            this.panelResumen.Controls.Add(this.lblTotalVentasLabel);
            this.panelResumen.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelResumen.Location = new System.Drawing.Point(0, 120);
            this.panelResumen.Name = "panelResumen";
            this.panelResumen.Size = new System.Drawing.Size(1400, 60);
            this.panelResumen.TabIndex = 2;
            // 
            // lblTotalVentasLabel
            // 
            this.lblTotalVentasLabel.AutoSize = true;
            this.lblTotalVentasLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalVentasLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblTotalVentasLabel.Location = new System.Drawing.Point(20, 20);
            this.lblTotalVentasLabel.Name = "lblTotalVentasLabel";
            this.lblTotalVentasLabel.Size = new System.Drawing.Size(85, 19);
            this.lblTotalVentasLabel.TabIndex = 0;
            this.lblTotalVentasLabel.Text = "Total Ventas:";
            // 
            // lblTotalVentas
            // 
            this.lblTotalVentas.AutoSize = true;
            this.lblTotalVentas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalVentas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotalVentas.Location = new System.Drawing.Point(120, 18);
            this.lblTotalVentas.Name = "lblTotalVentas";
            this.lblTotalVentas.Size = new System.Drawing.Size(55, 21);
            this.lblTotalVentas.TabIndex = 1;
            this.lblTotalVentas.Text = "$0.00";
            // 
            // lblTotalCantidadLabel
            // 
            this.lblTotalCantidadLabel.AutoSize = true;
            this.lblTotalCantidadLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalCantidadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblTotalCantidadLabel.Location = new System.Drawing.Point(300, 20);
            this.lblTotalCantidadLabel.Name = "lblTotalCantidadLabel";
            this.lblTotalCantidadLabel.Size = new System.Drawing.Size(103, 19);
            this.lblTotalCantidadLabel.TabIndex = 2;
            this.lblTotalCantidadLabel.Text = "Total Cantidad:";
            // 
            // lblTotalCantidad
            // 
            this.lblTotalCantidad.AutoSize = true;
            this.lblTotalCantidad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotalCantidad.Location = new System.Drawing.Point(420, 18);
            this.lblTotalCantidad.Name = "lblTotalCantidad";
            this.lblTotalCantidad.Size = new System.Drawing.Size(19, 21);
            this.lblTotalCantidad.TabIndex = 3;
            this.lblTotalCantidad.Text = "0";
            // 
            // lblTotalProductosLabel
            // 
            this.lblTotalProductosLabel.AutoSize = true;
            this.lblTotalProductosLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalProductosLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblTotalProductosLabel.Location = new System.Drawing.Point(600, 20);
            this.lblTotalProductosLabel.Name = "lblTotalProductosLabel";
            this.lblTotalProductosLabel.Size = new System.Drawing.Size(110, 19);
            this.lblTotalProductosLabel.TabIndex = 4;
            this.lblTotalProductosLabel.Text = "Total Productos:";
            // 
            // lblTotalProductos
            // 
            this.lblTotalProductos.AutoSize = true;
            this.lblTotalProductos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotalProductos.Location = new System.Drawing.Point(720, 18);
            this.lblTotalProductos.Name = "lblTotalProductos";
            this.lblTotalProductos.Size = new System.Drawing.Size(19, 21);
            this.lblTotalProductos.TabIndex = 5;
            this.lblTotalProductos.Text = "0";
            // 
            // panelGraficas
            // 
            this.panelGraficas.BackColor = System.Drawing.Color.White;
            this.panelGraficas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGraficas.Controls.Add(this.formsPlotCantidades);
            this.panelGraficas.Controls.Add(this.formsPlotVentas);
            this.panelGraficas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGraficas.Location = new System.Drawing.Point(0, 180);
            this.panelGraficas.Name = "panelGraficas";
            this.panelGraficas.Size = new System.Drawing.Size(1400, 350);
            this.panelGraficas.TabIndex = 3;
            // 
            // formsPlotVentas
            // 
            this.formsPlotVentas.Dock = System.Windows.Forms.DockStyle.Left;
            this.formsPlotVentas.Location = new System.Drawing.Point(0, 0);
            this.formsPlotVentas.Name = "formsPlotVentas";
            this.formsPlotVentas.Size = new System.Drawing.Size(700, 348);
            this.formsPlotVentas.TabIndex = 0;
            // 
            // formsPlotCantidades
            // 
            this.formsPlotCantidades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotCantidades.Location = new System.Drawing.Point(700, 0);
            this.formsPlotCantidades.Name = "formsPlotCantidades";
            this.formsPlotCantidades.Size = new System.Drawing.Size(698, 348);
            this.formsPlotCantidades.TabIndex = 1;
            // 
            // panelTabla
            // 
            this.panelTabla.Controls.Add(this.dgvReporte);
            this.panelTabla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTabla.Location = new System.Drawing.Point(0, 530);
            this.panelTabla.Name = "panelTabla";
            this.panelTabla.Size = new System.Drawing.Size(1400, 270);
            this.panelTabla.TabIndex = 4;
            // 
            // dgvReporte
            // 
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReporte.BackgroundColor = System.Drawing.Color.White;
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Producto", HeaderText = "Producto", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Cantidad", HeaderText = "Cantidad Vendida", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total Venta", ReadOnly = true }
            });
            this.dgvReporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReporte.Location = new System.Drawing.Point(0, 0);
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            this.dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.Size = new System.Drawing.Size(1400, 270);
            this.dgvReporte.TabIndex = 0;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Gray;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1250, 810);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(120, 40);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblEstado.Location = new System.Drawing.Point(20, 820);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(42, 15);
            this.lblEstado.TabIndex = 6;
            this.lblEstado.Text = "Listo";
            // 
            // ReporteVentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(1400, 860);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.panelTabla);
            this.Controls.Add(this.panelGraficas);
            this.Controls.Add(this.panelResumen);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.panelSuperior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReporteVentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Ventas - Super Esperanza";
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            this.panelResumen.ResumeLayout(false);
            this.panelResumen.PerformLayout();
            this.panelGraficas.ResumeLayout(false);
            this.panelTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelSuperior = null!;
        private System.Windows.Forms.Label lblTitulo = null!;
        private System.Windows.Forms.Panel panelControles = null!;
        private System.Windows.Forms.Label lblFecha = null!;
        private System.Windows.Forms.DateTimePicker dtpFecha = null!;
        private System.Windows.Forms.Button btnBuscar = null!;
        private System.Windows.Forms.Button btnExportarExcel = null!;
        private System.Windows.Forms.Panel panelResumen = null!;
        private System.Windows.Forms.Label lblTotalVentasLabel = null!;
        private System.Windows.Forms.Label lblTotalVentas = null!;
        private System.Windows.Forms.Label lblTotalCantidadLabel = null!;
        private System.Windows.Forms.Label lblTotalCantidad = null!;
        private System.Windows.Forms.Label lblTotalProductosLabel = null!;
        private System.Windows.Forms.Label lblTotalProductos = null!;
        private System.Windows.Forms.Panel panelGraficas = null!;
        private ScottPlot.WinForms.FormsPlot formsPlotVentas = null!;
        private ScottPlot.WinForms.FormsPlot formsPlotCantidades = null!;
        private System.Windows.Forms.Panel panelTabla = null!;
        private System.Windows.Forms.DataGridView dgvReporte = null!;
        private System.Windows.Forms.Button btnCerrar = null!;
        private System.Windows.Forms.Label lblEstado = null!;
    }
}

