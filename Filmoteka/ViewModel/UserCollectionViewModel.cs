using Filmoteka.Framework;
using Filmoteka.Model;
using Filmoteka.View.UserControls;
using Microsoft.EntityFrameworkCore;
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
    class UserCollectionViewModel : ViewModelBase
    {
        private UserViewModel? selectedUser;
        private UserViewModel? loggedUser;
        private string loginName = string.Empty;
        private string loginPassword = string.Empty;
        private string registrationName = string.Empty;
        private string registrationPassword = string.Empty;
        private string registrationPasswordVerification = string.Empty;
        private string message = string.Empty;
        private string registrationMessage = string.Empty;

        public ObservableCollection<UserViewModel> Users { get; set; }
        public UserViewModel? SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public UserViewModel? LoggedUser
        {
            get => loggedUser;
            set
            {
                loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
            }
        }
        public string LoginName
        {
            get => loginName;
            set
            {
                loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }
        public string LoginPassword
        {
            get => loginPassword;
            set
            {
                loginPassword = value;
                OnPropertyChanged(nameof(LoginPassword));
            }
        }
        public string RegistrationName
        {
            get => registrationName;
            set
            {
                registrationName = value;
                OnPropertyChanged(nameof(RegistrationName));
            }
        }
        public string RegistrationPassword
        {
            get => registrationPassword;
            set
            {
                registrationPassword = value;
                OnPropertyChanged(nameof(RegistrationPassword));
            }
        }
        public string RegistrationPasswordVerification
        {
            get => registrationPasswordVerification;
            set
            {
                registrationPasswordVerification = value;
                OnPropertyChanged(nameof(RegistrationPasswordVerification));
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public string RegistrationMessage
        {
            get => registrationMessage;
            set
            {
                registrationMessage = value;
                OnPropertyChanged(nameof(RegistrationMessage));
            }
        }
        public ICommand UserLogin => new RelayCommand(Login, CanLogin);
        public ICommand UserLogout => new RelayCommand(Logout, CanLogout);
        public ICommand UserRegistration => new RelayCommand(Register, CanRegister);

       
        public UserCollectionViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            using(MovieContext mc = new MovieContext())
            {
                foreach (User user in mc.Users.Include(x => x.UserMovies).ThenInclude(y => y.Movie))
                {
                    ObservableCollection<UserMovie> ratings = new ObservableCollection<UserMovie>();
                    foreach (UserMovie rating in user.UserMovies)
                    {
                        ratings.Add(rating);
                    }
                    UserViewModel userForViewModel = new UserViewModel { Id = user.Id, Name = user.Name, Password = user.Password, Ratings = ratings };
                    Users.Add(userForViewModel);
                }
            }
        }
        private bool CanLogin(object? arg)
        {
            return LoginName != string.Empty && LoginPassword != string.Empty && LoggedUser == null;
        }

        private void Login(object? obj)
        {
            foreach (UserViewModel user in Users)
            {
                if (user.Name == LoginName && user.Password == LoginPassword)
                {
                    LoggedUser = user;
                    Message = "Přihlášen: " + LoginName;
                    LoginName = string.Empty;
                    LoginPassword = string.Empty;
                    return;
                }
            }
            Message = "Chybné jméno či heslo";
            LoginName = string.Empty;
            LoginPassword = string.Empty;
        }
        private bool CanLogout(object? arg)
        {
            return LoggedUser != null;
        }

        private void Logout(object? obj)
        {
            LoggedUser = null;
            Message = string.Empty;
            RegistrationMessage = string.Empty;
        }
        private bool CanRegister(object? arg)
        {
            return RegistrationName != string.Empty && RegistrationPassword != string.Empty && RegistrationPasswordVerification != string.Empty;
        }

        private void Register(object? obj)
        {
            if (RegistrationPassword != RegistrationPasswordVerification)
            {
                RegistrationPassword = string.Empty;
                RegistrationPasswordVerification = string.Empty;
                RegistrationMessage = "Ověření hesla se nezdařilo," + Environment.NewLine + "zadejte prosím hesla znova";
                return;
            }
            using (MovieContext mc = new MovieContext())
            {
                mc.Users.Add(new User { Name = RegistrationName, Password = RegistrationPassword });
                mc.SaveChanges();
                var newUser = mc.Users.OrderBy(x => x.Id).Last();
                if (newUser.Name != RegistrationName)
                {
                    RegistrationName = string.Empty;
                    RegistrationPassword = string.Empty;
                    RegistrationPasswordVerification = string.Empty;
                    RegistrationMessage = "Nelze se připojit k databázi";
                }
                else
                {
                    ObservableCollection<UserMovie> ratings = new ObservableCollection<UserMovie>();
                    foreach (UserMovie rating in newUser.UserMovies)
                    {
                        ratings.Add(rating);
                    }
                    UserViewModel newUserViewModel = new UserViewModel { Id = newUser.Id, Name = newUser.Name, Password = newUser.Password, Ratings = ratings};
                    LoggedUser = newUserViewModel;
                    Users.Add(newUserViewModel);
                    Message = "Přihlášen: " + RegistrationName;
                    RegistrationName = string.Empty;
                    RegistrationPassword = string.Empty;
                    RegistrationPasswordVerification = string.Empty;
                    RegistrationMessage = "Registrace úspěšná, jste přihlášen";
                }
            }
        }

    }
}
