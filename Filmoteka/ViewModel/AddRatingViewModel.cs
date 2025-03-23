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
        public ICommand AddNewRating => new RelayCommand(AddRating);
        public AddRatingViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
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
                            movieCollectionViewModel.AddedMovie = (mc.Movies.Include(y => y.UserMovies).ThenInclude(z => z.User)).Where(x => x.Id == movieWithNewRating.Id).First();
                            mc.SaveChanges();
                            rating.Rating = NewDetailMovieRating;
                            rating.Review = NewDetailMovieReview;
                            rating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                            movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                            movieCollectionViewModel.SelectedMovie = movieWithNewRating;
                            movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                            movieCollectionViewModel.Movies.Clear();
                            movieCollectionViewModel.GetMoviesFromDatabase();
                            movieCollectionViewModel.SelectedMovie = movieWithNewRating;
                            ResetProperties();
                            return;
                        }
                    }
                    UserMovie newRating = new UserMovie{MovieId = movieWithNewRating.Id, UserId = LoggedUser.Id,
                        Rating = NewDetailMovieRating, Review = NewDetailMovieReview};
                    mc.UserMovies.Add(newRating);
                    movieCollectionViewModel.AddedMovie = (mc.Movies.Include(y => y.UserMovies).ThenInclude(z => z.User)).OrderBy(x => x.Id).Last();
                    mc.SaveChanges();
                    newRating.User = new User { Id = LoggedUser.Id, Name = LoggedUser.Name };
                    newRating.Movie = new Movie { Id = movieWithNewRating.Id, Name = movieWithNewRating.Name };
                    movieWithNewRating.Ratings.Add(newRating);
                    movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                    movieCollectionViewModel.SelectedMovie = movieWithNewRating;
                    movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                    movieCollectionViewModel.Movies.Clear();
                    movieCollectionViewModel.GetMoviesFromDatabase();
                    movieCollectionViewModel.SelectedMovie = movieWithNewRating;
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
