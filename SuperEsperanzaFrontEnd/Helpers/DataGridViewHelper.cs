using System.Windows.Forms;

namespace SuperEsperanzaFrontEnd.Helpers
{
    public static class DataGridViewHelper
    {
        // Paleta de colores "Esperanza Fresca"
        private static readonly System.Drawing.Color VerdePrincipal = System.Drawing.Color.FromArgb(42, 157, 143);
        private static readonly System.Drawing.Color AzulOscuro = System.Drawing.Color.FromArgb(38, 70, 83);
        private static readonly System.Drawing.Color AcentoBrillante = System.Drawing.Color.FromArgb(233, 196, 106);
        private static readonly System.Drawing.Color Fondo = System.Drawing.Color.FromArgb(244, 244, 244);
        private static readonly System.Drawing.Color Blanco = System.Drawing.Color.White;

        public static void AplicarEstiloModerno(DataGridView dgv)
        {
            // Configuración general
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Blanco;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.EnableHeadersVisualStyles = false;

            // Estilos de encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = AzulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Blanco;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Estilos de celdas
            dgv.DefaultCellStyle.BackColor = Blanco;
            dgv.DefaultCellStyle.ForeColor = AzulOscuro;
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.DefaultCellStyle.SelectionBackColor = VerdePrincipal;
            dgv.DefaultCellStyle.SelectionForeColor = Blanco;

            // Estilos de filas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Fondo;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = AzulOscuro;

            // Altura de filas
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 40;
            
            // Mejoras visuales adicionales
            dgv.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }
    }
}

