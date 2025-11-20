#nullable enable
namespace SuperEsperanzaFrontEnd.Vistas
{
    partial class PuntoVentaForm
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
            this.lblSesion = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.lblCarrito = new System.Windows.Forms.Label();
            this.dgvCarrito = new System.Windows.Forms.DataGridView();
            this.btnActualizarItem = new System.Windows.Forms.Button();
            this.btnEliminarItem = new System.Windows.Forms.Button();
            this.btnLimpiarCarrito = new System.Windows.Forms.Button();
            this.panelTotales = new System.Windows.Forms.Panel();
            this.lblSubtotalLabel = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblDescuentoLabel = new System.Windows.Forms.Label();
            this.lblDescuento = new System.Windows.Forms.Label();
            this.lblImpuestoLabel = new System.Windows.Forms.Label();
            this.lblImpuesto = new System.Windows.Forms.Label();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnFinalizarVenta = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.panelSuperior.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            this.panelTotales.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.panelSuperior.Controls.Add(this.lblSesion);
            this.panelSuperior.Controls.Add(this.lblTitulo);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1400, 60);
            this.panelSuperior.TabIndex = 0;
            // 
            // lblSesion
            // 
            this.lblSesion.AutoSize = true;
            this.lblSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSesion.ForeColor = System.Drawing.Color.White;
            this.lblSesion.Location = new System.Drawing.Point(1200, 20);
            this.lblSesion.Name = "lblSesion";
            this.lblSesion.Size = new System.Drawing.Size(50, 19);
            this.lblSesion.TabIndex = 1;
            this.lblSesion.Text = "Sesión:";
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
            this.lblTitulo.Text = "Punto de Venta";
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelIzquierdo.Controls.Add(this.lblBuscar);
            this.panelIzquierdo.Controls.Add(this.txtBuscar);
            this.panelIzquierdo.Controls.Add(this.btnBuscar);
            this.panelIzquierdo.Controls.Add(this.dgvProductos);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 60);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(700, 640);
            this.panelIzquierdo.TabIndex = 1;
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblBuscar.Location = new System.Drawing.Point(20, 20);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(52, 19);
            this.lblBuscar.TabIndex = 3;
            this.lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtBuscar.Location = new System.Drawing.Point(20, 45);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(550, 25);
            this.txtBuscar.TabIndex = 2;
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscar_KeyDown);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(580, 45);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 25);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dgvProductos
            // 
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Categoria", HeaderText = "Categoría", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Precio", HeaderText = "Precio", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Stock", HeaderText = "Stock", ReadOnly = true }
            });
            this.dgvProductos.Location = new System.Drawing.Point(20, 80);
            this.dgvProductos.MultiSelect = false;
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.ReadOnly = true;
            this.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.Size = new System.Drawing.Size(660, 540);
            this.dgvProductos.TabIndex = 0;
            this.dgvProductos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductos_CellDoubleClick);
            this.dgvProductos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProductos_KeyDown);
            // 
            // panelDerecho
            // 
            this.panelDerecho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelDerecho.Controls.Add(this.lblEstado);
            this.panelDerecho.Controls.Add(this.btnFinalizarVenta);
            this.panelDerecho.Controls.Add(this.panelTotales);
            this.panelDerecho.Controls.Add(this.btnLimpiarCarrito);
            this.panelDerecho.Controls.Add(this.btnEliminarItem);
            this.panelDerecho.Controls.Add(this.btnActualizarItem);
            this.panelDerecho.Controls.Add(this.dgvCarrito);
            this.panelDerecho.Controls.Add(this.lblCarrito);
            this.panelDerecho.Controls.Add(this.cmbCliente);
            this.panelDerecho.Controls.Add(this.lblCliente);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(700, 60);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Size = new System.Drawing.Size(700, 640);
            this.panelDerecho.TabIndex = 2;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblCliente.Location = new System.Drawing.Point(20, 20);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(55, 19);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente:";
            // 
            // cmbCliente
            // 
            this.cmbCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(20, 45);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(660, 25);
            this.cmbCliente.TabIndex = 1;
            this.cmbCliente.SelectedIndexChanged += new System.EventHandler(this.cmbCliente_SelectedIndexChanged);
            // 
            // lblCarrito
            // 
            this.lblCarrito.AutoSize = true;
            this.lblCarrito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCarrito.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblCarrito.Location = new System.Drawing.Point(20, 85);
            this.lblCarrito.Name = "lblCarrito";
            this.lblCarrito.Size = new System.Drawing.Size(60, 21);
            this.lblCarrito.TabIndex = 2;
            this.lblCarrito.Text = "Carrito:";
            // 
            // dgvCarrito
            // 
            this.dgvCarrito.AllowUserToAddRows = false;
            this.dgvCarrito.AllowUserToDeleteRows = false;
            this.dgvCarrito.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCarrito.BackgroundColor = System.Drawing.Color.White;
            this.dgvCarrito.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCarrito.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Producto", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Cantidad", HeaderText = "Cant.", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Precio", HeaderText = "Precio", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Subtotal", HeaderText = "Subtotal", ReadOnly = true }
            });
            this.dgvCarrito.Location = new System.Drawing.Point(20, 110);
            this.dgvCarrito.MultiSelect = false;
            this.dgvCarrito.Name = "dgvCarrito";
            this.dgvCarrito.ReadOnly = true;
            this.dgvCarrito.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarrito.Size = new System.Drawing.Size(660, 250);
            this.dgvCarrito.TabIndex = 3;
            this.dgvCarrito.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCarrito_CellDoubleClick);
            // 
            // btnActualizarItem
            // 
            this.btnActualizarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.btnActualizarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnActualizarItem.ForeColor = System.Drawing.Color.White;
            this.btnActualizarItem.Location = new System.Drawing.Point(20, 370);
            this.btnActualizarItem.Name = "btnActualizarItem";
            this.btnActualizarItem.Size = new System.Drawing.Size(120, 30);
            this.btnActualizarItem.TabIndex = 4;
            this.btnActualizarItem.Text = "Actualizar Item";
            this.btnActualizarItem.UseVisualStyleBackColor = false;
            this.btnActualizarItem.Click += new System.EventHandler(this.btnActualizarItem_Click);
            // 
            // btnEliminarItem
            // 
            this.btnEliminarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(111)))), ((int)(((byte)(81)))));
            this.btnEliminarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEliminarItem.ForeColor = System.Drawing.Color.White;
            this.btnEliminarItem.Location = new System.Drawing.Point(150, 370);
            this.btnEliminarItem.Name = "btnEliminarItem";
            this.btnEliminarItem.Size = new System.Drawing.Size(120, 30);
            this.btnEliminarItem.TabIndex = 5;
            this.btnEliminarItem.Text = "Eliminar Item";
            this.btnEliminarItem.UseVisualStyleBackColor = false;
            this.btnEliminarItem.Click += new System.EventHandler(this.btnEliminarItem_Click);
            // 
            // btnLimpiarCarrito
            // 
            this.btnLimpiarCarrito.BackColor = System.Drawing.Color.Gray;
            this.btnLimpiarCarrito.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarCarrito.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLimpiarCarrito.ForeColor = System.Drawing.Color.White;
            this.btnLimpiarCarrito.Location = new System.Drawing.Point(280, 370);
            this.btnLimpiarCarrito.Name = "btnLimpiarCarrito";
            this.btnLimpiarCarrito.Size = new System.Drawing.Size(120, 30);
            this.btnLimpiarCarrito.TabIndex = 6;
            this.btnLimpiarCarrito.Text = "Limpiar Carrito";
            this.btnLimpiarCarrito.UseVisualStyleBackColor = false;
            this.btnLimpiarCarrito.Click += new System.EventHandler(this.btnLimpiarCarrito_Click);
            // 
            // panelTotales
            // 
            this.panelTotales.BackColor = System.Drawing.Color.White;
            this.panelTotales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTotales.Controls.Add(this.lblTotal);
            this.panelTotales.Controls.Add(this.lblTotalLabel);
            this.panelTotales.Controls.Add(this.lblImpuesto);
            this.panelTotales.Controls.Add(this.lblImpuestoLabel);
            this.panelTotales.Controls.Add(this.lblDescuento);
            this.panelTotales.Controls.Add(this.lblDescuentoLabel);
            this.panelTotales.Controls.Add(this.lblSubtotal);
            this.panelTotales.Controls.Add(this.lblSubtotalLabel);
            this.panelTotales.Location = new System.Drawing.Point(20, 410);
            this.panelTotales.Name = "panelTotales";
            this.panelTotales.Size = new System.Drawing.Size(660, 120);
            this.panelTotales.TabIndex = 6;
            // 
            // lblSubtotalLabel
            // 
            this.lblSubtotalLabel.AutoSize = true;
            this.lblSubtotalLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtotalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblSubtotalLabel.Location = new System.Drawing.Point(20, 15);
            this.lblSubtotalLabel.Name = "lblSubtotalLabel";
            this.lblSubtotalLabel.Size = new System.Drawing.Size(65, 19);
            this.lblSubtotalLabel.TabIndex = 0;
            this.lblSubtotalLabel.Text = "Subtotal:";
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblSubtotal.Location = new System.Drawing.Point(550, 15);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(42, 19);
            this.lblSubtotal.TabIndex = 1;
            this.lblSubtotal.Text = "$0.00";
            // 
            // lblDescuentoLabel
            // 
            this.lblDescuentoLabel.AutoSize = true;
            this.lblDescuentoLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDescuentoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblDescuentoLabel.Location = new System.Drawing.Point(20, 40);
            this.lblDescuentoLabel.Name = "lblDescuentoLabel";
            this.lblDescuentoLabel.Size = new System.Drawing.Size(73, 19);
            this.lblDescuentoLabel.TabIndex = 2;
            this.lblDescuentoLabel.Text = "Descuento:";
            // 
            // lblDescuento
            // 
            this.lblDescuento.AutoSize = true;
            this.lblDescuento.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblDescuento.Location = new System.Drawing.Point(550, 40);
            this.lblDescuento.Name = "lblDescuento";
            this.lblDescuento.Size = new System.Drawing.Size(42, 19);
            this.lblDescuento.TabIndex = 3;
            this.lblDescuento.Text = "$0.00";
            // 
            // lblImpuestoLabel
            // 
            this.lblImpuestoLabel.AutoSize = true;
            this.lblImpuestoLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblImpuestoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblImpuestoLabel.Location = new System.Drawing.Point(20, 65);
            this.lblImpuestoLabel.Name = "lblImpuestoLabel";
            this.lblImpuestoLabel.Size = new System.Drawing.Size(70, 19);
            this.lblImpuestoLabel.TabIndex = 4;
            this.lblImpuestoLabel.Text = "Impuesto:";
            // 
            // lblImpuesto
            // 
            this.lblImpuesto.AutoSize = true;
            this.lblImpuesto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblImpuesto.Location = new System.Drawing.Point(550, 65);
            this.lblImpuesto.Name = "lblImpuesto";
            this.lblImpuesto.Size = new System.Drawing.Size(42, 19);
            this.lblImpuesto.TabIndex = 5;
            this.lblImpuesto.Text = "$0.00";
            // 
            // lblTotalLabel
            // 
            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotalLabel.Location = new System.Drawing.Point(20, 90);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(58, 25);
            this.lblTotalLabel.TabIndex = 6;
            this.lblTotalLabel.Text = "TOTAL:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblTotal.Location = new System.Drawing.Point(550, 90);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(55, 25);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "$0.00";
            // 
            // btnFinalizarVenta
            // 
            this.btnFinalizarVenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.btnFinalizarVenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizarVenta.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnFinalizarVenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.btnFinalizarVenta.Location = new System.Drawing.Point(20, 550);
            this.btnFinalizarVenta.Name = "btnFinalizarVenta";
            this.btnFinalizarVenta.Size = new System.Drawing.Size(660, 50);
            this.btnFinalizarVenta.TabIndex = 7;
            this.btnFinalizarVenta.Text = "FINALIZAR VENTA";
            this.btnFinalizarVenta.UseVisualStyleBackColor = false;
            this.btnFinalizarVenta.Click += new System.EventHandler(this.btnFinalizarVenta_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblEstado.Location = new System.Drawing.Point(20, 610);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(42, 15);
            this.lblEstado.TabIndex = 8;
            this.lblEstado.Text = "Listo";
            // 
            // PuntoVentaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(1400, 700);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelSuperior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PuntoVentaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Punto de Venta - Super Esperanza";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.panelTotales.ResumeLayout(false);
            this.panelTotales.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelSuperior = null!;
        private System.Windows.Forms.Label lblTitulo = null!;
        private System.Windows.Forms.Label lblSesion = null!;
        private System.Windows.Forms.Panel panelIzquierdo = null!;
        private System.Windows.Forms.DataGridView dgvProductos = null!;
        private System.Windows.Forms.Label lblBuscar = null!;
        private System.Windows.Forms.TextBox txtBuscar = null!;
        private System.Windows.Forms.Button btnBuscar = null!;
        private System.Windows.Forms.Panel panelDerecho = null!;
        private System.Windows.Forms.Label lblCliente = null!;
        private System.Windows.Forms.ComboBox cmbCliente = null!;
        private System.Windows.Forms.Label lblCarrito = null!;
        private System.Windows.Forms.DataGridView dgvCarrito = null!;
        private System.Windows.Forms.Button btnActualizarItem = null!;
        private System.Windows.Forms.Button btnEliminarItem = null!;
        private System.Windows.Forms.Button btnLimpiarCarrito = null!;
        private System.Windows.Forms.Panel panelTotales = null!;
        private System.Windows.Forms.Label lblSubtotalLabel = null!;
        private System.Windows.Forms.Label lblSubtotal = null!;
        private System.Windows.Forms.Label lblDescuentoLabel = null!;
        private System.Windows.Forms.Label lblDescuento = null!;
        private System.Windows.Forms.Label lblImpuestoLabel = null!;
        private System.Windows.Forms.Label lblImpuesto = null!;
        private System.Windows.Forms.Label lblTotalLabel = null!;
        private System.Windows.Forms.Label lblTotal = null!;
        private System.Windows.Forms.Button btnFinalizarVenta = null!;
        private System.Windows.Forms.Label lblEstado = null!;

        private void txtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
        }
    }
}

