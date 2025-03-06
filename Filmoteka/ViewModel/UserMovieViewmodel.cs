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
        private UserViewModel userViewModel;
        private MovieViewModel movieViewModel;

        public UserMovieViewmodel(UserViewModel userViewModel, MovieViewModel movieViewModel)
        {
            this.userViewModel = userViewModel;
            this.movieViewModel = movieViewModel;
            userViewModel.PropertyChanged += UserViewModel_PropertyChanged;
        }

        private void UserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userViewModel.LoggedUser != movieViewModel.LoggedUser)
            {
                movieViewModel.LoggedUser = userViewModel.LoggedUser;
            }
        }
    }
}
