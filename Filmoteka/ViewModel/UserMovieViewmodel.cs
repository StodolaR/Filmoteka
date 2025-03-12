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
    class UserMovieViewmodel : ViewModelBase
    {
        private MovieCollectionViewModel movieCollectionViewModel;
        private UserCollectionViewModel userCollectionViewModel;
        public UserMovieViewmodel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel)
        {
            this.userCollectionViewModel = userCollectionViewModel;
            this.movieCollectionViewModel = movieCollectionViewModel;
            userCollectionViewModel.PropertyChanged += UserCollectionViewModel_PropertyChanged;
            movieCollectionViewModel.Movies.CollectionChanged += Movies_CollectionChanged;
        }

        private void UserCollectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(userCollectionViewModel.LoggedUser != movieCollectionViewModel.LoggedUser)
            {
                movieCollectionViewModel.LoggedUser = userCollectionViewModel.LoggedUser;
            }
        }

        private void Movies_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                MovieViewModel newRatedMovie = new MovieViewModel();
                newRatedMovie = (MovieViewModel)(e.NewItems[0]);
                UserMovie? movieRatingInLoggedUserRatings = userCollectionViewModel.LoggedUser.Ratings.Where(x => x.MovieId == newRatedMovie.Id).First();
                if (movieRatingInLoggedUserRatings != null)
                {
                    userCollectionViewModel.LoggedUser.Ratings.Remove(movieRatingInLoggedUserRatings);
                }
                userCollectionViewModel.LoggedUser.Ratings.Add(newRatedMovie.Ratings.Last());
            } 
        }      
    }
}
