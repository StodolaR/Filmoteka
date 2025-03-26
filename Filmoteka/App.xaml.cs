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
                if (!Directory.Exists(@"Zaloha-ukazka/Posters"))
                {
                    Directory.CreateDirectory("Zaloha-ukazka/Posters");
                    File.Copy(@"../../../../Zaloha-ukazka/movies.db", @"Zaloha-ukazka/movies.db", true);
                    string[] paths = Directory.GetFiles("..\\..\\..\\..\\Zaloha-ukazka\\Posters");
                    foreach (string path in paths)
                    {
                        string target = Path.Combine("Zaloha-ukazka/Posters", Path.GetFileName(path));
                        File.Copy(path, target, true);
                    }
                }
                if (!Directory.Exists("Zaloha-prazdna"))
                {
                    Directory.CreateDirectory("Zaloha-prazdna");
                    File.Copy(@"../../../../Zaloha-prazdna/movies.db", @"Zaloha-prazdna/movies.db");
                }
                if (!File.Exists("movies.db"))
                {
                    File.Copy(@"Zaloha-ukazka/movies.db","movies.db");
                }
                if (!Directory.Exists("Posters"))
                {
                    Directory.CreateDirectory("Posters");
                    string[] paths = Directory.GetFiles("Zaloha-ukazka\\Posters");
                    foreach (string path in paths)
                    {
                        string target = Path.Combine("Posters", Path.GetFileName(path));
                        File.Copy(path, target, true);
                    }
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Databáze nenalezena, aplikace bude ukončena {ex}");
                Application.Current.Shutdown();
            }
            
        }
    }

}
