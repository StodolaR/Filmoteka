using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Filmoteka.Model;
using Filmoteka.ViewModel;

namespace Filmoteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserViewModel userViewModel = (UserViewModel)Application.Current.FindResource("userViewModel");
            MovieViewModel movieViewModel = new MovieViewModel(userViewModel);
            DataContext = movieViewModel;
            
            //using (MovieContext mc = new MovieContext())
            //{
            //    mc.Movies.Add(new Movie()
            //    {
            //        Name = "Rambo",
            //        Description = "Válečný veterán po návratu domů bojuje s policií",
            //        Genre = GenreType.Akční,
            //        Year = 1983,
            //        PicturePath = "Posters/Rambo.jpg"
            //    });
            //    mc.Movies.Add(new Movie()
            //    {
            //        Name = "Thing",
            //        Description = "Vědce za polárním kruhem likviduje neznámý organismus",
            //        Genre = GenreType.Horor,
            //        Year = 1976,
            //        PicturePath = "Posters/Thing.jpg"
            //    });
            //    mc.Movies.Add(new Movie()
            //    {
            //        Name = "Žhavé výstřely",
            //        Description = "Parodie na Top Gun",
            //        Genre = GenreType.Komedie,
            //        Year = 1991,
            //        PicturePath = "Posters/ZhaveVystrely.jpg"
            //    });
            //    mc.Users.Add(new User { Name = "Admin", Password = "Admin" });
            //    mc.Users.Add(new User { Name = "Uzivatel1", Password = "Uzivatel123" });
            //    mc.Users.Add(new User { Name = "Pepa", Password = "Pepa123" });
            //    mc.SaveChanges();
            //}
        }
    }
}