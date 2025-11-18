namespace SuperEsperanzaFrontEnd.Vistas
{
    partial class SesionCajaForm
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
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblMontoInicial = new System.Windows.Forms.Label();
            this.txtMontoInicial = new System.Windows.Forms.TextBox();
            this.lblCodigoSesion = new System.Windows.Forms.Label();
            this.txtCodigoSesion = new System.Windows.Forms.TextBox();
            this.btnAbrirSesion = new System.Windows.Forms.Button();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.lblSesionesActivas = new System.Windows.Forms.Label();
            this.dgvSesiones = new System.Windows.Forms.DataGridView();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.panelSuperior.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSesiones)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.panelSuperior.Controls.Add(this.lblTitulo);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1000, 60);
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
            this.lblTitulo.Text = "Sesiones de Caja";
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelIzquierdo.Controls.Add(this.lblObservacion);
            this.panelIzquierdo.Controls.Add(this.txtObservacion);
            this.panelIzquierdo.Controls.Add(this.lblMontoInicial);
            this.panelIzquierdo.Controls.Add(this.txtMontoInicial);
            this.panelIzquierdo.Controls.Add(this.lblCodigoSesion);
            this.panelIzquierdo.Controls.Add(this.txtCodigoSesion);
            this.panelIzquierdo.Controls.Add(this.btnAbrirSesion);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 60);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(400, 540);
            this.panelIzquierdo.TabIndex = 1;
            // 
            // lblCodigoSesion
            // 
            this.lblCodigoSesion.AutoSize = true;
            this.lblCodigoSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCodigoSesion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblCodigoSesion.Location = new System.Drawing.Point(20, 30);
            this.lblCodigoSesion.Name = "lblCodigoSesion";
            this.lblCodigoSesion.Size = new System.Drawing.Size(200, 19);
            this.lblCodigoSesion.TabIndex = 0;
            this.lblCodigoSesion.Text = "Código Sesión (opcional):";
            // 
            // txtCodigoSesion
            // 
            this.txtCodigoSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCodigoSesion.Location = new System.Drawing.Point(20, 55);
            this.txtCodigoSesion.Name = "txtCodigoSesion";
            this.txtCodigoSesion.Size = new System.Drawing.Size(360, 25);
            this.txtCodigoSesion.TabIndex = 1;
            // 
            // lblMontoInicial
            // 
            this.lblMontoInicial.AutoSize = true;
            this.lblMontoInicial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMontoInicial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblMontoInicial.Location = new System.Drawing.Point(20, 100);
            this.lblMontoInicial.Name = "lblMontoInicial";
            this.lblMontoInicial.Size = new System.Drawing.Size(95, 19);
            this.lblMontoInicial.TabIndex = 2;
            this.lblMontoInicial.Text = "Monto Inicial:";
            // 
            // txtMontoInicial
            // 
            this.txtMontoInicial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMontoInicial.Location = new System.Drawing.Point(20, 125);
            this.txtMontoInicial.Name = "txtMontoInicial";
            this.txtMontoInicial.Size = new System.Drawing.Size(360, 25);
            this.txtMontoInicial.TabIndex = 3;
            this.txtMontoInicial.Text = "0";
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblObservacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblObservacion.Location = new System.Drawing.Point(20, 170);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(85, 19);
            this.lblObservacion.TabIndex = 4;
            this.lblObservacion.Text = "Observación:";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtObservacion.Location = new System.Drawing.Point(20, 195);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(360, 100);
            this.txtObservacion.TabIndex = 5;
            // 
            // btnAbrirSesion
            // 
            this.btnAbrirSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(196)))), ((int)(((byte)(106)))));
            this.btnAbrirSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirSesion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAbrirSesion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.btnAbrirSesion.Location = new System.Drawing.Point(20, 320);
            this.btnAbrirSesion.Name = "btnAbrirSesion";
            this.btnAbrirSesion.Size = new System.Drawing.Size(360, 50);
            this.btnAbrirSesion.TabIndex = 6;
            this.btnAbrirSesion.Text = "ABRIR SESIÓN";
            this.btnAbrirSesion.UseVisualStyleBackColor = false;
            this.btnAbrirSesion.Click += new System.EventHandler(this.btnAbrirSesion_Click);
            // 
            // panelDerecho
            // 
            this.panelDerecho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelDerecho.Controls.Add(this.lblEstado);
            this.panelDerecho.Controls.Add(this.btnCerrar);
            this.panelDerecho.Controls.Add(this.btnCerrarSesion);
            this.panelDerecho.Controls.Add(this.dgvSesiones);
            this.panelDerecho.Controls.Add(this.lblSesionesActivas);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(400, 60);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Size = new System.Drawing.Size(600, 540);
            this.panelDerecho.TabIndex = 2;
            // 
            // lblSesionesActivas
            // 
            this.lblSesionesActivas.AutoSize = true;
            this.lblSesionesActivas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSesionesActivas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(157)))), ((int)(((byte)(143)))));
            this.lblSesionesActivas.Location = new System.Drawing.Point(20, 20);
            this.lblSesionesActivas.Name = "lblSesionesActivas";
            this.lblSesionesActivas.Size = new System.Drawing.Size(140, 21);
            this.lblSesionesActivas.TabIndex = 0;
            this.lblSesionesActivas.Text = "Sesiones Activas:";
            // 
            // dgvSesiones
            // 
            this.dgvSesiones.AllowUserToAddRows = false;
            this.dgvSesiones.AllowUserToDeleteRows = false;
            this.dgvSesiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSesiones.BackgroundColor = System.Drawing.Color.White;
            this.dgvSesiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSesiones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", ReadOnly = true, Visible = false },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Usuario", HeaderText = "Usuario", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "FechaApertura", HeaderText = "Fecha Apertura", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "MontoInicial", HeaderText = "Monto Inicial", ReadOnly = true },
            new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", ReadOnly = true }
            });
            this.dgvSesiones.Location = new System.Drawing.Point(20, 50);
            this.dgvSesiones.MultiSelect = false;
            this.dgvSesiones.Name = "dgvSesiones";
            this.dgvSesiones.ReadOnly = true;
            this.dgvSesiones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSesiones.Size = new System.Drawing.Size(560, 350);
            this.dgvSesiones.TabIndex = 1;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(111)))), ((int)(((byte)(81)))));
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(20, 420);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(270, 40);
            this.btnCerrarSesion.TabIndex = 2;
            this.btnCerrarSesion.Text = "Cerrar Sesión Seleccionada";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Gray;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(310, 420);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(270, 40);
            this.btnCerrar.TabIndex = 3;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(70)))), ((int)(((byte)(83)))));
            this.lblEstado.Location = new System.Drawing.Point(20, 480);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(42, 15);
            this.lblEstado.TabIndex = 4;
            this.lblEstado.Text = "Listo";
            // 
            // SesionCajaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelSuperior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SesionCajaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Sesiones de Caja - Super Esperanza";
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSesiones)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelSuperior;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Label lblCodigoSesion;
        private System.Windows.Forms.TextBox txtCodigoSesion;
        private System.Windows.Forms.Label lblMontoInicial;
        private System.Windows.Forms.TextBox txtMontoInicial;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Button btnAbrirSesion;
        private System.Windows.Forms.Panel panelDerecho;
        private System.Windows.Forms.Label lblSesionesActivas;
        private System.Windows.Forms.DataGridView dgvSesiones;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblEstado;
    }
}

