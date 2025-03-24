using Filmoteka.Framework;
using Filmoteka.Model;
using Microsoft.EntityFrameworkCore;
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
        private string? newDetailMovieReview;
        public int NewDetailMovieRating
        {
            get => newDetailMovieRating;
            set
            {
                newDetailMovieRating = value;
                OnPropertyChanged(nameof(NewDetailMovieRating));
            }
        }
        public string? NewDetailMovieReview
        {
            get => newDetailMovieReview;
            set
            {
                newDetailMovieReview = value;
                OnPropertyChanged(nameof(NewDetailMovieReview));
            }
        }
        public ICommand AddNewRating => new RelayCommand(AddRating);
        public AddRatingViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
        }
        private void AddRating(object? obj)
        {
            if (movieCollectionViewModel.SelectedMovie != null && LoggedUser != null)
            {
                MovieViewModel movieWithNewRating = movieCollectionViewModel.SelectedMovie;
                foreach (UserMovie rating in movieWithNewRating.Ratings)
                {
                    if (rating.UserId == LoggedUser.Id)
                    {
                        EditRating(movieWithNewRating, rating);
                        return;
                    }
                }
                NewRating(movieWithNewRating);
            }
        }
        private void EditRating(MovieViewModel movieWithNewRating, UserMovie rating)
        {
            if (LoggedUser != null)
            {
                using (MovieContext mc = new MovieContext())
                {
                    mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == movieWithNewRating.Id).First().Rating = NewDetailMovieRating;
                    mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == movieWithNewRating.Id).First().Review = NewDetailMovieReview;
                    movieCollectionViewModel.AddedMovie = mc.Movies.Where(x => x.Id == movieWithNewRating.Id).First();
                    mc.SaveChanges();
                }
                rating.Rating = NewDetailMovieRating;
                rating.Review = NewDetailMovieReview;
                rating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                ActualizeViews(movieWithNewRating);
            }
        }
        private void NewRating(MovieViewModel movieWithNewRating)
        {
            if (LoggedUser != null)
            {
                using (MovieContext mc = new MovieContext())
                {
                    UserMovie newRating = new UserMovie{MovieId = movieWithNewRating.Id, UserId = LoggedUser.Id,
                        Rating = NewDetailMovieRating,Review = NewDetailMovieReview};
                    mc.UserMovies.Add(newRating);
                    movieCollectionViewModel.AddedMovie = mc.Movies.Where(x => x.Id == movieWithNewRating.Id).First();
                    mc.SaveChanges();
                    newRating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                    movieWithNewRating.Ratings.Add(newRating);
                    ActualizeViews(movieWithNewRating);
                }
            }
        }
        private void ActualizeViews(MovieViewModel movieWithNewRating)
        {
            movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
            movieCollectionViewModel.Movies.Clear();
            movieCollectionViewModel.GetMoviesFromDatabase();
            if (movieCollectionViewModel.SelectedMovie != null)
            {
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
            }
            movieCollectionViewModel.SelectedMovie = movieWithNewRating;
            movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
            NewDetailMovieReview = null;
            NewDetailMovieRating = 0;
        }
    }
}
