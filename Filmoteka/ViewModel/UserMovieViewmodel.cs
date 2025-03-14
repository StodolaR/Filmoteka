using Filmoteka.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Filmoteka.Model;

namespace Filmoteka.ViewModel
{
    abstract class UserMovieViewmodel : ViewModelBase
    {
        protected UserCollectionViewModel userCollectionViewModel;
        protected MovieCollectionViewModel movieCollectionViewModel;
        private UserViewModel? loggedUser;

        public UserViewModel? LoggedUser 
        {
            get => loggedUser;
            set 
            {
                loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
            } 
        }

        protected UserMovieViewmodel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel)
        {
            this.userCollectionViewModel = userCollectionViewModel;
            this.movieCollectionViewModel = movieCollectionViewModel;
            movieCollectionViewModel.Movies.CollectionChanged += Movies_CollectionChanged;
            userCollectionViewModel.PropertyChanged += UserCollectionViewModel_PropertyChanged;
        }

        private void UserCollectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userCollectionViewModel.LoggedUser != LoggedUser)
            {
                LoggedUser = userCollectionViewModel.LoggedUser;
            }
        }

        protected void Movies_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                MovieViewModel newRatedMovie = new MovieViewModel();
                newRatedMovie = (MovieViewModel)(e.NewItems[0]);
                UserMovie? movieRatingInLoggedUserRatings = LoggedUser.Ratings.Where(x => x.MovieId == newRatedMovie.Id).FirstOrDefault();
                if (movieRatingInLoggedUserRatings != null)
                {
                    LoggedUser.Ratings.Remove(movieRatingInLoggedUserRatings);
                }
                LoggedUser.Ratings.Add(newRatedMovie.Ratings.Where(x => x.UserId == LoggedUser.Id).First());
            } 
        }      
    }
}
