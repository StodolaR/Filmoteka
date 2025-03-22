using Filmoteka.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Filmoteka.View.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        private string[]? searchStrings;
        public SearchControl()
        {
            InitializeComponent();
        }
        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            MovieViewModel movie = (MovieViewModel)e.Item;
            if (movie != null && searchStrings != null)
            {
                bool isAccepted = false;
                foreach (string searchString in searchStrings)
                {
                    if (movie.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isAccepted = true;
                    }
                }
                e.Accepted = isAccepted;
            }
            else 
            {
                e.Accepted = false; 
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                var movies = (ObservableCollection<MovieViewModel>)((CollectionViewSource)searchControl.FindResource("movies")).Source;
                if (movies.Any(x => x.Name.Contains(tbSearch.Text, StringComparison.InvariantCultureIgnoreCase)))
                {
                    searchStrings = new string[] { tbSearch.Text };
                }
                else
                {
                    searchStrings = tbSearch.Text.Split(" ");
                }
                lbSearch.SelectedItem = null;
                CollectionViewSource.GetDefaultView(lbSearch.ItemsSource).Refresh();
                PopupSearchListbox.IsOpen = true;
                if (!lbSearch.HasItems)
                {
                    tbSearch.Text = "";
                    PopupSearch.IsOpen = true;
                }
            }
        }
        private void lbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSearch.Text = "";
            PopupSearchListbox.IsOpen = false;
        }
    }
}
