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
        private MovieCollectionViewModel movieCollectionViewModel;
        private UserCollectionViewModel userCollectionViewModel;

        public UserCollectionViewModel UserCollectionViewModel { get => userCollectionViewModel; set => userCollectionViewModel = value; }

        public UserMovieViewmodel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel)
        {
            this.userCollectionViewModel = userCollectionViewModel;
            this.movieCollectionViewModel = movieCollectionViewModel;
            userCollectionViewModel.PropertyChanged += UserCollectionViewModel_PropertyChanged;
        }

        private void UserCollectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userCollectionViewModel.LoggedUser != movieCollectionViewModel.LoggedUser)
            {
                movieCollectionViewModel.LoggedUser = userCollectionViewModel.LoggedUser;
            }
        }
    }
}
