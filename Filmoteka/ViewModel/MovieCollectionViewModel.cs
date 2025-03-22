using Filmoteka.Framework;
using Filmoteka.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    class MovieCollectionViewModel : ViewModelBase
    {
        private MovieViewModel? selectedMovie;
        private MovieViewModel? selectedSearchedMovie;
        public ObservableCollection<MovieViewModel> Movies { get; set; }
        public MovieViewModel? SelectedMovie
        {
            get => selectedMovie;
            set
            {
                selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }
        public MovieViewModel? SelectedSearchedMovie 
        {
            get => selectedSearchedMovie;
            set 
            {
                selectedSearchedMovie = value;
                if (value != null)
                {
                    SelectedMovie = value;
                }
            } 
        }
        public MovieCollectionViewModel()
        {
            Movies = new ObservableCollection<MovieViewModel>();
            using (MovieContext mc = new MovieContext())
            {
                foreach (Movie movie in mc.Movies.Include(x => x.UserMovies).ThenInclude(y => y.User))
                {
                    ObservableCollection<UserMovie> ratings = new ObservableCollection<UserMovie>();
                    foreach (UserMovie rating in movie.UserMovies)
                    {
                        ratings.Add(rating);
                    }
                    int avgRating = (int)(ratings.Average(x => x.Rating)*20);
                    MovieViewModel movieForViewModel = new MovieViewModel {Id = movie.Id, AvgRating = avgRating, Description = movie.Description,
                     Genre = movie.Genre, Name = movie.Name, PicturePath = movie.PicturePath, Ratings = ratings, Year = movie.Year};
                    Movies.Add(movieForViewModel);
                }
            }
        }
        
        
    }
}
