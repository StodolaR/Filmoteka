using Filmoteka.Framework;
using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    public class UzivatelViewModel 
    {
        public ObservableCollection<User> Users { get; set; }
        public User? LoggedUser { get; set; }
        public ICommand UserLogin => new RelayCommand(Login, CanLogin);
        public UzivatelViewModel()
        {
            Users = new ObservableCollection<User>();
            using(MovieContext mc = new MovieContext())
            {
                foreach (User user in mc.Users)
                {
                    Users.Add(user);
                }
            }
        }
        private bool CanLogin(object? arg)
        {
            throw new NotImplementedException();
        }

        private void Login(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}
