using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Filmoteka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "movies.db")))
                {
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Zaloha-prazdna", "movies.db"),
                        Path.Combine(Directory.GetCurrentDirectory(), "movies.db"));
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch
            {
                MessageBox.Show("Databáze nenalezena, aplikace bude ukončena");
                Application.Current.Shutdown();
            }
            
        }
    }

}
