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
            UserCollectionViewModel userCollectionViewModel = (UserCollectionViewModel)Application.Current.FindResource("userCollectionViewModel");
            MovieCollectionViewModel movieCollectionViewModel = (MovieCollectionViewModel)Application.Current.FindResource("movieCollectionViewModel");
            UserMovieViewmodel userMovieViewmodel = new UserMovieViewmodel(userCollectionViewModel, movieCollectionViewModel);
        }
    }
}