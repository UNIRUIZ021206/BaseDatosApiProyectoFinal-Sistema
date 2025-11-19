using SuperEsperanzaFrontEnd.Services;

namespace SuperEsperanzaFrontEnd.Vistas
{
    public partial class MainForm : Form
    {
        // Paleta de colores "Esperanza Fresca"
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);   // #2A9D8F
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);        // #264653
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106); // #E9C46A
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);         // #F4F4F4
        private static readonly System.Drawing.Color Alerta = System.Drawing.Color.FromArgb(231, 111, 81);          // #E76F51

        public MainForm()
        {
            InitializeComponent();
            CargarInformacionUsuario();
            ActualizarFecha();
            AplicarPermisos();
            this.FormClosing += MainForm_FormClosing;
        }

        private void AplicarPermisos()
        {
            // Aplicar permisos según el rol del usuario
            // El botón Productos abre el Punto de Venta, así que verifica ambos permisos
            btnProductos.Enabled = PermissionService.TieneAcceso("Productos") && PermissionService.PuedeUsarPuntoVenta();
            btnCategorias.Enabled = PermissionService.TieneAcceso("Categorias");
            btnClientes.Enabled = PermissionService.TieneAcceso("Clientes");
            btnProveedores.Enabled = PermissionService.TieneAcceso("Proveedores");
            btnCompras.Enabled = PermissionService.TieneAcceso("Compras");
            // El botón Facturas abre reportes de ventas, requiere permisos específicos
            btnFacturas.Enabled = PermissionService.PuedeVerReporteVentas();
            // El botón Reportes abre reportes de ventas con gráficas, requiere permisos específicos
            btnReportes.Enabled = PermissionService.PuedeVerReporteVentas();
            // El botón Lotes abre reportes de inventario, requiere permisos específicos
            btnLotes.Enabled = PermissionService.PuedeVerReporteInventario();
            btnSesiones.Enabled = PermissionService.TieneAcceso("Sesiones");
            btnUsuarios.Enabled = PermissionService.TieneAcceso("Usuarios");
            btnRoles.Enabled = PermissionService.TieneAcceso("Roles");

            // Si no tiene acceso, cambiar el color del botón para indicarlo
            if (!btnProductos.Enabled) btnProductos.BackColor = System.Drawing.Color.Gray;
            if (!btnCategorias.Enabled) btnCategorias.BackColor = System.Drawing.Color.Gray;
            if (!btnClientes.Enabled) btnClientes.BackColor = System.Drawing.Color.Gray;
            if (!btnProveedores.Enabled) btnProveedores.BackColor = System.Drawing.Color.Gray;
            if (!btnCompras.Enabled) btnCompras.BackColor = System.Drawing.Color.Gray;
            if (!btnFacturas.Enabled) btnFacturas.BackColor = System.Drawing.Color.Gray;
            if (!btnReportes.Enabled) btnReportes.BackColor = System.Drawing.Color.Gray;
            if (!btnLotes.Enabled) btnLotes.BackColor = System.Drawing.Color.Gray;
            if (!btnSesiones.Enabled) btnSesiones.BackColor = System.Drawing.Color.Gray;
            if (!btnUsuarios.Enabled) btnUsuarios.BackColor = System.Drawing.Color.Gray;
            if (!btnRoles.Enabled) btnRoles.BackColor = System.Drawing.Color.Gray;
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Si el usuario cierra la ventana (X), confirmar
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var resultado = MessageBox.Show(
                    "¿Está seguro que desea salir de la aplicación?",
                    "Salir",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    TokenService.CerrarSesion();
                }
            }
        }

        private void CargarInformacionUsuario()
        {
            var usuario = TokenService.ObtenerUsuario();
            var rol = TokenService.ObtenerRol();

            if (!string.IsNullOrEmpty(usuario))
            {
                lblBienvenida.Text = $"¡Bienvenido, {usuario}!";
                lblUsuarioStatus.Text = $"Usuario: {usuario}";
            }
            else
            {
                lblBienvenida.Text = "¡Bienvenido!";
                lblUsuarioStatus.Text = "Usuario: No identificado";
            }

            if (!string.IsNullOrEmpty(rol))
            {
                lblRolStatus.Text = $"Rol: {rol}";
            }
            else
            {
                lblRolStatus.Text = "Rol: No asignado";
            }
        }

        private void ActualizarFecha()
        {
            lblFechaStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            
            // Actualizar la fecha cada segundo
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => lblFechaStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
        }

        private void btnProductos_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.PuedeUsarPuntoVenta())
            {
                MessageBox.Show("No tiene permisos para usar el Punto de Venta.\n\nSolo Administradores y Cajeros pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir Punto de Venta (POS)
            using (var posForm = new PuntoVentaForm())
            {
                posForm.ShowDialog();
            }
        }

        private void btnCategorias_Click(object? sender, EventArgs e)
        {
            MostrarMensaje("Módulo de Categorías - Próximamente");
        }

        private void btnClientes_Click(object? sender, EventArgs e)
        {
            MostrarMensaje("Módulo de Clientes - Próximamente");
        }

        private void btnProveedores_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.TieneAcceso("Proveedores"))
            {
                MessageBox.Show("No tiene permisos para acceder a Proveedores.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de proveedores
            using (var proveedoresForm = new ProveedoresForm())
            {
                proveedoresForm.ShowDialog();
            }
        }

        private void btnCompras_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.TieneAcceso("Compras") || !PermissionService.PuedeCrear("Compras"))
            {
                MessageBox.Show("No tiene permisos para crear compras.\n\nSolo Administradores y Bodegueros pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de compras
            using (var comprasForm = new ComprasForm())
            {
                comprasForm.ShowDialog();
            }
        }

        private void btnFacturas_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.PuedeVerReporteVentas())
            {
                MessageBox.Show("No tiene permisos para ver reportes de ventas.\n\nSolo Administradores, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de reporte de ventas
            using (var reporteForm = new ReporteVentasForm())
            {
                reporteForm.ShowDialog();
            }
        }

        private void btnReportes_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.PuedeVerReporteVentas())
            {
                MessageBox.Show("No tiene permisos para ver reportes de ventas.\n\nSolo Administradores, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de reporte de ventas con gráficas
            using (var reporteForm = new ReporteVentasForm())
            {
                reporteForm.ShowDialog();
            }
        }

        private void btnLotes_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.PuedeVerReporteInventario())
            {
                MessageBox.Show("No tiene permisos para ver reportes de inventario.\n\nSolo Administradores, Bodegueros, Supervisores, Gerentes y Contadores pueden acceder a esta funcionalidad.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de inventario general
            using (var inventarioForm = new InventarioForm())
            {
                inventarioForm.ShowDialog();
            }
        }

        private void btnSesiones_Click(object? sender, EventArgs e)
        {
            // Verificar permisos antes de abrir
            if (!PermissionService.TieneAcceso("Sesiones"))
            {
                MessageBox.Show("No tiene permisos para acceder a Sesiones de Caja.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abrir formulario de gestión de sesiones de caja
            using (var sesionForm = new SesionCajaForm())
            {
                sesionForm.ShowDialog();
            }
        }

        private void btnUsuarios_Click(object? sender, EventArgs e)
        {
            MostrarMensaje("Módulo de Usuarios - Próximamente");
        }

        private void btnRoles_Click(object? sender, EventArgs e)
        {
            MostrarMensaje("Módulo de Roles - Próximamente");
        }

        private void btnCerrarSesion_Click(object? sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Está seguro que desea cerrar sesión?",
                "Cerrar Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                TokenService.CerrarSesion();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            lblBienvenida.Text = mensaje;
            lblBienvenida.ForeColor = AzulOscuro;
        }
    }
}
