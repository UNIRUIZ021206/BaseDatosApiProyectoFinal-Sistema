using SuperEsperanzaFrontEnd.Vistas;

namespace SuperEsperanzaFrontEnd
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Ciclo de login - volver al login si se cierra sesión
            while (true)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Abrir el formulario principal después del login exitoso
                        using (var mainForm = new MainForm())
                        {
                            if (mainForm.ShowDialog() == DialogResult.OK)
                            {
                                // Si se cerró sesión, volver al login
                                continue;
                            }
                            else
                            {
                                // Si se cerró la aplicación, salir
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Si se canceló el login, salir
                        break;
                    }
                }
            }
        }
    }
}