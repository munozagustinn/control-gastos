using System;
using System.IO;
using System.Windows.Forms;
using ControlGastos.Backend.Persistence;
using ControlGastos.Backend.Services;

namespace ControlGastos.WinForms
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string carpetaDatos = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ControlGastos");
            string archivoDatos = Path.Combine(carpetaDatos, "gastos.json");

            try
            {
                ServicioGastos servicio = new ServicioGastos(new RepositorioGastosJson(archivoDatos));
                Application.Run(new MainForm(servicio));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo iniciar la aplicación.\n\n" + ex.Message,
                    "Control de Gastos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
