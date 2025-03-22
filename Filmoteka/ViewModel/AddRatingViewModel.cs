using Filmoteka.Framework;
using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    class AddRatingViewModel : UserMovieViewmodel
    {       
        private int newDetailMovieRating;
        private string newDetailMovieReview = string.Empty;
        public int NewDetailMovieRating
        {
            get => newDetailMovieRating;
            set
            {
                newDetailMovieRating = value;
                OnPropertyChanged(nameof(NewDetailMovieRating));
            }
        }
        public string NewDetailMovieReview
        {
            get => newDetailMovieReview;
            set
            {
                newDetailMovieReview = value;
                OnPropertyChanged(nameof(NewDetailMovieReview));
            }
        }
        public ICommand AddNewRating => new RelayCommand(AddRating, CanAddRating);
        public AddRatingViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
        }
        private void UserCollectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userCollectionViewModel.LoggedUser != LoggedUser)
            {
                LoggedUser = userCollectionViewModel.LoggedUser;
            }
        }
        private bool CanAddRating(object? arg)
        {
            return true;
        }
        private void AddRating(object? obj)
        {
            if (movieCollectionViewModel.SelectedMovie != null && LoggedUser != null)
            {
                using (MovieContext mc = new MovieContext())
                {
                    MovieViewModel movieWithNewRating = movieCollectionViewModel.SelectedMovie;
                    movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                    foreach (UserMovie rating in movieWithNewRating.Ratings)
                    {
                        if (rating.UserId == LoggedUser.Id)
                        {
                            mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == movieWithNewRating.Id).First().Rating = NewDetailMovieRating;
                            mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == movieWithNewRating.Id).First().Review = NewDetailMovieReview;
                            mc.SaveChanges();
                            rating.Rating = NewDetailMovieRating;
                            rating.Review = NewDetailMovieReview;
                            rating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                            movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                            movieCollectionViewModel.SelectedMovie = movieWithNewRating;
                            movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                            ResetProperties();
                            return;
                        }
                    }
                    UserMovie newRating = new UserMovie
                    {
                        MovieId = movieWithNewRating.Id,
                        UserId = LoggedUser.Id,
                        Rating = NewDetailMovieRating,
                        Review = NewDetailMovieReview
                    };
                    mc.UserMovies.Add(newRating);
                    mc.SaveChanges();
                    newRating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                    newRating.Movie = new Movie { Id = movieWithNewRating.Id, Name = movieWithNewRating.Name };
                    movieWithNewRating.Ratings.Add(newRating);
                    movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                    movieCollectionViewModel.SelectedMovie = movieWithNewRating;
                    movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                    ResetProperties();
                }
            }
        }
        private void ResetProperties()
        {
            NewDetailMovieReview = string.Empty;
            NewDetailMovieRating = 0;
        }
    }
}
