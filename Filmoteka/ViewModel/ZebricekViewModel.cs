using Filmoteka.Framework;
using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZebricekViewModel : ViewModelBase
    {
        private string newMovieName = string.Empty;
        private GenreType newMovieGenre;
        private string newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string newMoviePicturePath = "Cesta k obrázku";

        public ObservableCollection<Movie> Movies { get; set; }
        public string NewMovieName
        {
            get => newMovieName;
            set
            {
                newMovieName = value;
                OnPropertyChanged(nameof(NewMovieName));
            }
        }
        public GenreType NewMovieGenre
        {
            get => newMovieGenre;
            set 
            {
                newMovieGenre = value;
                OnPropertyChanged(nameof(newMovieGenre));
            }
        }
        public string NewMovieDescription
        {
            get => newMovieDescription;
            set 
            {
                newMovieDescription = value;
                OnPropertyChanged(nameof(NewMovieDescription));
            }
        }
        public string NewMovieYear
        {
            get => newMovieYear;
            set
            {
                newMovieYear = value;
                OnPropertyChanged(nameof(NewMovieYear));
            }
        }
        public string NewMoviePicturePath
        {
            get => newMoviePicturePath;
            set 
            {
                newMoviePicturePath = value;
                OnPropertyChanged(nameof(NewMoviePicturePath));
            }
        }
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
