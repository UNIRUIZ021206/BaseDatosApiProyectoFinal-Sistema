#nullable enable
namespace SuperEsperanzaFrontEnd.Vistas
{
    partial class InventarioForm
    {
        private System.ComponentModel.IContainer? components = null;

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
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.panelResumen = new System.Windows.Forms.Panel();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.lblValorTotalLabel = new System.Windows.Forms.Label();
            this.lblStockBajo = new System.Windows.Forms.Label();
            this.lblStockBajoLabel = new System.Windows.Forms.Label();
            this.lblProductosActivos = new System.Windows.Forms.Label();
            this.lblProductosActivosLabel = new System.Windows.Forms.Label();
            this.lblTotalProductos = new System.Windows.Forms.Label();
            this.lblTotalProductosLabel = new System.Windows.Forms.Label();
            this.dgvInventario = new System.Windows.Forms.DataGridView();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.panelSuperior.SuspendLayout();
            this.panelControles.SuspendLayout();
            this.panelResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.panelSuperior.Controls.Add(this.lblTitulo);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1200, 60);
            this.panelSuperior.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Inventario General";
            // 
            // panelControles
            // 
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelControles.Controls.Add(this.btnRefrescar);
            this.panelControles.Controls.Add(this.btnBuscar);
            this.panelControles.Controls.Add(this.txtBuscar);
            this.panelControles.Controls.Add(this.lblBuscar);
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControles.Location = new System.Drawing.Point(0, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(1200, 60);
            this.panelControles.TabIndex = 1;
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblBuscar.Location = new System.Drawing.Point(20, 20);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(52, 19);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtBuscar.Location = new System.Drawing.Point(80, 18);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(400, 25);
            this.txtBuscar.TabIndex = 1;
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscar_KeyDown);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(500, 18);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 30);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.btnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRefrescar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.btnRefrescar.Location = new System.Drawing.Point(640, 18);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(120, 30);
            this.btnRefrescar.TabIndex = 3;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = false;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // panelResumen
            // 
            this.panelResumen.BackColor = System.Drawing.Color.White;
            this.panelResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelResumen.Controls.Add(this.lblValorTotal);
            this.panelResumen.Controls.Add(this.lblValorTotalLabel);
            this.panelResumen.Controls.Add(this.lblStockBajo);
            this.panelResumen.Controls.Add(this.lblStockBajoLabel);
            this.panelResumen.Controls.Add(this.lblProductosActivos);
            this.panelResumen.Controls.Add(this.lblProductosActivosLabel);
            this.panelResumen.Controls.Add(this.lblTotalProductos);
            this.panelResumen.Controls.Add(this.lblTotalProductosLabel);
            this.panelResumen.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelResumen.Location = new System.Drawing.Point(0, 120);
            this.panelResumen.Name = "panelResumen";
            this.panelResumen.Size = new System.Drawing.Size(1200, 60);
            this.panelResumen.TabIndex = 2;
            // 
            // lblTotalProductosLabel
            // 
            this.lblTotalProductosLabel.AutoSize = true;
            this.lblTotalProductosLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalProductosLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblTotalProductosLabel.Location = new System.Drawing.Point(20, 20);
            this.lblTotalProductosLabel.Name = "lblTotalProductosLabel";
            this.lblTotalProductosLabel.Size = new System.Drawing.Size(110, 19);
            this.lblTotalProductosLabel.TabIndex = 0;
            this.lblTotalProductosLabel.Text = "Total Productos:";
            // 
            // lblTotalProductos
            // 
            this.lblTotalProductos.AutoSize = true;
            this.lblTotalProductos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotalProductos.Location = new System.Drawing.Point(140, 18);
            this.lblTotalProductos.Name = "lblTotalProductos";
            this.lblTotalProductos.Size = new System.Drawing.Size(19, 21);
            this.lblTotalProductos.TabIndex = 1;
            this.lblTotalProductos.Text = "0";
            // 
            // lblProductosActivosLabel
            // 
            this.lblProductosActivosLabel.AutoSize = true;
            this.lblProductosActivosLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblProductosActivosLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblProductosActivosLabel.Location = new System.Drawing.Point(250, 20);
            this.lblProductosActivosLabel.Name = "lblProductosActivosLabel";
            this.lblProductosActivosLabel.Size = new System.Drawing.Size(120, 19);
            this.lblProductosActivosLabel.TabIndex = 2;
            this.lblProductosActivosLabel.Text = "Productos Activos:";
            // 
            // lblProductosActivos
            // 
            this.lblProductosActivos.AutoSize = true;
            this.lblProductosActivos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProductosActivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblProductosActivos.Location = new System.Drawing.Point(380, 18);
            this.lblProductosActivos.Name = "lblProductosActivos";
            this.lblProductosActivos.Size = new System.Drawing.Size(19, 21);
            this.lblProductosActivos.TabIndex = 3;
            this.lblProductosActivos.Text = "0";
            // 
            // lblStockBajoLabel
            // 
            this.lblStockBajoLabel.AutoSize = true;
            this.lblStockBajoLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStockBajoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblStockBajoLabel.Location = new System.Drawing.Point(500, 20);
            this.lblStockBajoLabel.Name = "lblStockBajoLabel";
            this.lblStockBajoLabel.Size = new System.Drawing.Size(80, 19);
            this.lblStockBajoLabel.TabIndex = 4;
            this.lblStockBajoLabel.Text = "Stock Bajo:";
            // 
            // lblStockBajo
            // 
            this.lblStockBajo.AutoSize = true;
            this.lblStockBajo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStockBajo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(111)))), ((int)(((byte)(81)))));
            this.lblStockBajo.Location = new System.Drawing.Point(590, 18);
            this.lblStockBajo.Name = "lblStockBajo";
            this.lblStockBajo.Size = new System.Drawing.Size(19, 21);
            this.lblStockBajo.TabIndex = 5;
            this.lblStockBajo.Text = "0";
            // 
            // lblValorTotalLabel
            // 
            this.lblValorTotalLabel.AutoSize = true;
            this.lblValorTotalLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValorTotalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblValorTotalLabel.Location = new System.Drawing.Point(750, 20);
            this.lblValorTotalLabel.Name = "lblValorTotalLabel";
            this.lblValorTotalLabel.Size = new System.Drawing.Size(80, 19);
            this.lblValorTotalLabel.TabIndex = 6;
            this.lblValorTotalLabel.Text = "Valor Total:";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.AutoSize = true;
            this.lblValorTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblValorTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblValorTotal.Location = new System.Drawing.Point(840, 18);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(55, 21);
            this.lblValorTotal.TabIndex = 7;
            this.lblValorTotal.Text = "$0.00";
            // 
            // dgvInventario
            // 
            this.dgvInventario.AllowUserToAddRows = false;
            this.dgvInventario.AllowUserToDeleteRows = false;
            this.dgvInventario.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInventario.BackgroundColor = System.Drawing.Color.White;
            this.dgvInventario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventario.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Producto", HeaderText = "Producto", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Categoria", HeaderText = "Categoría", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Precio", HeaderText = "Precio Venta", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "StockActual", HeaderText = "Stock Actual", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "StockLotes", HeaderText = "Stock en Lotes", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Diferencia", HeaderText = "Diferencia", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Alerta", HeaderText = "", ReadOnly = true }
            });
            this.dgvInventario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInventario.Location = new System.Drawing.Point(0, 180);
            this.dgvInventario.Name = "dgvInventario";
            this.dgvInventario.ReadOnly = true;
            this.dgvInventario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInventario.Size = new System.Drawing.Size(1200, 450);
            this.dgvInventario.TabIndex = 3;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Gray;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1050, 640);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(120, 40);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblEstado.Location = new System.Drawing.Point(20, 650);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(42, 15);
            this.lblEstado.TabIndex = 5;
            this.lblEstado.Text = "Listo";
            // 
            // InventarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.dgvInventario);
            this.Controls.Add(this.panelResumen);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.panelSuperior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventario General - Super Esperanza";
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            this.panelResumen.ResumeLayout(false);
            this.panelResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelSuperior = null!;
        private System.Windows.Forms.Label lblTitulo = null!;
        private System.Windows.Forms.Panel panelControles = null!;
        private System.Windows.Forms.Label lblBuscar = null!;
        private System.Windows.Forms.TextBox txtBuscar = null!;
        private System.Windows.Forms.Button btnBuscar = null!;
        private System.Windows.Forms.Button btnRefrescar = null!;
        private System.Windows.Forms.Panel panelResumen = null!;
        private System.Windows.Forms.Label lblTotalProductosLabel = null!;
        private System.Windows.Forms.Label lblTotalProductos = null!;
        private System.Windows.Forms.Label lblProductosActivosLabel = null!;
        private System.Windows.Forms.Label lblProductosActivos = null!;
        private System.Windows.Forms.Label lblStockBajoLabel = null!;
        private System.Windows.Forms.Label lblStockBajo = null!;
        private System.Windows.Forms.Label lblValorTotalLabel = null!;
        private System.Windows.Forms.Label lblValorTotal = null!;
        private System.Windows.Forms.DataGridView dgvInventario = null!;
        private System.Windows.Forms.Button btnCerrar = null!;
        private System.Windows.Forms.Label lblEstado = null!;
    }
}

