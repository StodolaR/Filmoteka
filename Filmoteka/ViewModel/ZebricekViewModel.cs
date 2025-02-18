using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZebricekViewModel 
    {
        public ObservableCollection<Movie> Movies { get; set; } 
        
        public ZebricekViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            using (MovieContext mc = new MovieContext())
            {
                foreach (Movie movie in mc.Movies)
                {
                    Movies.Add(movie);
                }
            }

        }
        
    }
}
