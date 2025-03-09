using Filmoteka.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class UserMovieViewmodel : ViewModelBase
    {
        private UserCollectionViewModel userCollectionViewModel;
        private MovieCollectionViewModel movieViewModel;

        public UserMovieViewmodel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieViewModel)
        {
            this.userCollectionViewModel = userCollectionViewModel;
            this.movieViewModel = movieViewModel;
            userCollectionViewModel.PropertyChanged += UserCollectionViewModel_PropertyChanged;
        }

        private void UserCollectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userCollectionViewModel.LoggedUser != movieViewModel.LoggedUser)
            {
                movieViewModel.LoggedUser = userCollectionViewModel.LoggedUser;
            }
        }
    }
}
