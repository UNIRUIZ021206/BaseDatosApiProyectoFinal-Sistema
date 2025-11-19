namespace SuperEsperanzaFrontEnd.Vistas
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnCerrarSesion = new Button();
            btnRoles = new Button();
            btnUsuarios = new Button();
            btnSesiones = new Button();
            btnLotes = new Button();
            btnReportes = new Button();
            btnFacturas = new Button();
            btnCompras = new Button();
            btnProveedores = new Button();
            btnClientes = new Button();
            btnCategorias = new Button();
            btnProductos = new Button();
            lblTituloMenu = new Label();
            panelContenido = new Panel();
            lblBienvenida = new Label();
            statusStrip = new StatusStrip();
            lblUsuarioStatus = new ToolStripStatusLabel();
            lblRolStatus = new ToolStripStatusLabel();
            lblFechaStatus = new ToolStripStatusLabel();
            panelMenu.SuspendLayout();
            panelContenido.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(38, 70, 83);
            panelMenu.Controls.Add(lblTituloMenu);
            panelMenu.Controls.Add(btnProductos);
            panelMenu.Controls.Add(btnCategorias);
            panelMenu.Controls.Add(btnClientes);
            panelMenu.Controls.Add(btnProveedores);
            panelMenu.Controls.Add(btnCompras);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnFacturas);
            panelMenu.Controls.Add(btnLotes);
            panelMenu.Controls.Add(btnSesiones);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnRoles);
            panelMenu.Controls.Add(btnCerrarSesion);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(3, 4, 3, 4);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(251, 800);
            panelMenu.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.BackColor = Color.FromArgb(231, 111, 81);
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(11, 776);
            btnCerrarSesion.Margin = new Padding(3, 4, 3, 4);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(229, 53);
            btnCerrarSesion.TabIndex = 11;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // btnRoles
            // 
            btnRoles.BackColor = Color.FromArgb(42, 157, 143);
            btnRoles.FlatStyle = FlatStyle.Flat;
            btnRoles.Font = new Font("Segoe UI", 10F);
            btnRoles.ForeColor = Color.White;
            btnRoles.Location = new Point(11, 709);
            btnRoles.Margin = new Padding(3, 4, 3, 4);
            btnRoles.Name = "btnRoles";
            btnRoles.Size = new Size(229, 53);
            btnRoles.TabIndex = 10;
            btnRoles.Text = "Roles";
            btnRoles.UseVisualStyleBackColor = false;
            btnRoles.Click += btnRoles_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(42, 157, 143);
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI", 10F);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(11, 642);
            btnUsuarios.Margin = new Padding(3, 4, 3, 4);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(229, 53);
            btnUsuarios.TabIndex = 9;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.UseVisualStyleBackColor = false;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnSesiones
            // 
            btnSesiones.BackColor = Color.FromArgb(42, 157, 143);
            btnSesiones.FlatStyle = FlatStyle.Flat;
            btnSesiones.Font = new Font("Segoe UI", 10F);
            btnSesiones.ForeColor = Color.White;
            btnSesiones.Location = new Point(11, 575);
            btnSesiones.Margin = new Padding(3, 4, 3, 4);
            btnSesiones.Name = "btnSesiones";
            btnSesiones.Size = new Size(229, 53);
            btnSesiones.TabIndex = 8;
            btnSesiones.Text = "Sesiones";
            btnSesiones.UseVisualStyleBackColor = false;
            btnSesiones.Click += btnSesiones_Click;
            // 
            // btnLotes
            // 
            btnLotes.BackColor = Color.FromArgb(42, 157, 143);
            btnLotes.FlatStyle = FlatStyle.Flat;
            btnLotes.Font = new Font("Segoe UI", 10F);
            btnLotes.ForeColor = Color.White;
            btnLotes.Location = new Point(11, 508);
            btnLotes.Margin = new Padding(3, 4, 3, 4);
            btnLotes.Name = "btnLotes";
            btnLotes.Size = new Size(229, 53);
            btnLotes.TabIndex = 7;
            btnLotes.Text = "Lotes";
            btnLotes.UseVisualStyleBackColor = false;
            btnLotes.Click += btnLotes_Click;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(233, 196, 106);
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReportes.ForeColor = Color.FromArgb(38, 70, 83);
            btnReportes.Location = new Point(11, 374);
            btnReportes.Margin = new Padding(3, 4, 3, 4);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(229, 53);
            btnReportes.TabIndex = 12;
            btnReportes.Text = "Reportes";
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Visible = true;
            btnReportes.Enabled = true;
            btnReportes.Click += btnReportes_Click;
            // 
            // btnFacturas
            // 
            btnFacturas.BackColor = Color.FromArgb(42, 157, 143);
            btnFacturas.FlatStyle = FlatStyle.Flat;
            btnFacturas.Font = new Font("Segoe UI", 10F);
            btnFacturas.ForeColor = Color.White;
            btnFacturas.Location = new Point(11, 441);
            btnFacturas.Margin = new Padding(3, 4, 3, 4);
            btnFacturas.Name = "btnFacturas";
            btnFacturas.Size = new Size(229, 53);
            btnFacturas.TabIndex = 6;
            btnFacturas.Text = "Facturas";
            btnFacturas.UseVisualStyleBackColor = false;
            btnFacturas.Click += btnFacturas_Click;
            // 
            // btnCompras
            // 
            btnCompras.BackColor = Color.FromArgb(42, 157, 143);
            btnCompras.FlatStyle = FlatStyle.Flat;
            btnCompras.Font = new Font("Segoe UI", 10F);
            btnCompras.ForeColor = Color.White;
            btnCompras.Location = new Point(11, 307);
            btnCompras.Margin = new Padding(3, 4, 3, 4);
            btnCompras.Name = "btnCompras";
            btnCompras.Size = new Size(229, 53);
            btnCompras.TabIndex = 5;
            btnCompras.Text = "Compras";
            btnCompras.UseVisualStyleBackColor = false;
            btnCompras.Click += btnCompras_Click;
            // 
            // btnProveedores
            // 
            btnProveedores.BackColor = Color.FromArgb(42, 157, 143);
            btnProveedores.FlatStyle = FlatStyle.Flat;
            btnProveedores.Font = new Font("Segoe UI", 10F);
            btnProveedores.ForeColor = Color.White;
            btnProveedores.Location = new Point(11, 240);
            btnProveedores.Margin = new Padding(3, 4, 3, 4);
            btnProveedores.Name = "btnProveedores";
            btnProveedores.Size = new Size(229, 53);
            btnProveedores.TabIndex = 4;
            btnProveedores.Text = "Proveedores";
            btnProveedores.UseVisualStyleBackColor = false;
            btnProveedores.Click += btnProveedores_Click;
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.FromArgb(42, 157, 143);
            btnClientes.FlatStyle = FlatStyle.Flat;
            btnClientes.Font = new Font("Segoe UI", 10F);
            btnClientes.ForeColor = Color.White;
            btnClientes.Location = new Point(11, 174);
            btnClientes.Margin = new Padding(3, 4, 3, 4);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(229, 53);
            btnClientes.TabIndex = 3;
            btnClientes.Text = "Clientes";
            btnClientes.UseVisualStyleBackColor = false;
            btnClientes.Click += btnClientes_Click;
            // 
            // btnCategorias
            // 
            btnCategorias.BackColor = Color.FromArgb(42, 157, 143);
            btnCategorias.FlatStyle = FlatStyle.Flat;
            btnCategorias.Font = new Font("Segoe UI", 10F);
            btnCategorias.ForeColor = Color.White;
            btnCategorias.Location = new Point(11, 107);
            btnCategorias.Margin = new Padding(3, 4, 3, 4);
            btnCategorias.Name = "btnCategorias";
            btnCategorias.Size = new Size(229, 53);
            btnCategorias.TabIndex = 2;
            btnCategorias.Text = "Categorías";
            btnCategorias.UseVisualStyleBackColor = false;
            btnCategorias.Click += btnCategorias_Click;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(233, 196, 106);
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProductos.ForeColor = Color.FromArgb(38, 70, 83);
            btnProductos.Location = new Point(11, 40);
            btnProductos.Margin = new Padding(3, 4, 3, 4);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(229, 53);
            btnProductos.TabIndex = 1;
            btnProductos.Text = "Productos";
            btnProductos.UseVisualStyleBackColor = false;
            btnProductos.Click += btnProductos_Click;
            // 
            // lblTituloMenu
            // 
            lblTituloMenu.AutoSize = true;
            lblTituloMenu.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloMenu.ForeColor = Color.White;
            lblTituloMenu.Location = new Point(11, 0);
            lblTituloMenu.Name = "lblTituloMenu";
            lblTituloMenu.Size = new Size(0, 28);
            lblTituloMenu.TabIndex = 0;
            // 
            // panelContenido
            // 
            panelContenido.BackColor = Color.FromArgb(244, 244, 244);
            panelContenido.Controls.Add(lblBienvenida);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(251, 0);
            panelContenido.Margin = new Padding(3, 4, 3, 4);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(1120, 774);
            panelContenido.TabIndex = 1;
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.FromArgb(42, 157, 143);
            lblBienvenida.Location = new Point(57, 67);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(0, 54);
            lblBienvenida.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.FromArgb(244, 244, 244);
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblUsuarioStatus, lblRolStatus, lblFechaStatus });
            statusStrip.Location = new Point(251, 774);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1120, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // lblUsuarioStatus
            // 
            lblUsuarioStatus.Font = new Font("Segoe UI", 9F);
            lblUsuarioStatus.ForeColor = Color.FromArgb(38, 70, 83);
            lblUsuarioStatus.Name = "lblUsuarioStatus";
            lblUsuarioStatus.Size = new Size(62, 20);
            lblUsuarioStatus.Text = "Usuario:";
            // 
            // lblRolStatus
            // 
            lblRolStatus.Font = new Font("Segoe UI", 9F);
            lblRolStatus.ForeColor = Color.FromArgb(38, 70, 83);
            lblRolStatus.Name = "lblRolStatus";
            lblRolStatus.Size = new Size(34, 20);
            lblRolStatus.Text = "Rol:";
            // 
            // lblFechaStatus
            // 
            lblFechaStatus.Font = new Font("Segoe UI", 9F);
            lblFechaStatus.ForeColor = Color.FromArgb(38, 70, 83);
            lblFechaStatus.Name = "lblFechaStatus";
            lblFechaStatus.Size = new Size(1007, 20);
            lblFechaStatus.Spring = true;
            lblFechaStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 244, 244);
            ClientSize = new Size(1371, 800);
            Controls.Add(panelContenido);
            Controls.Add(statusStrip);
            Controls.Add(panelMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Super Esperanza - Sistema de Gestión";
            WindowState = FormWindowState.Maximized;
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            panelContenido.ResumeLayout(false);
            panelContenido.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Label lblTituloMenu;
        private System.Windows.Forms.Button btnProductos;
        private System.Windows.Forms.Button btnCategorias;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnProveedores;
        private System.Windows.Forms.Button btnCompras;
        private System.Windows.Forms.Button btnFacturas;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Button btnLotes;
        private System.Windows.Forms.Button btnSesiones;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnRoles;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblUsuarioStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRolStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblFechaStatus;
    }
}