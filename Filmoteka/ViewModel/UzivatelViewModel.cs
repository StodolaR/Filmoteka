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
    public class UzivatelViewModel : ViewModelBase
    {
        private User? loggedUser;
        private string loginName = string.Empty;
        private string loginPassword = string.Empty;
        private string registerName = string.Empty;
        private string registerPassword = string.Empty;
        private string registerPasswordVerification = string.Empty;
        private string message = string.Empty;

        public ObservableCollection<User> Users { get; set; }
        public User? LoggedUser
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
        public string RegisterName
        {
            get => registerName;
            set
            {
                registerName = value;
                OnPropertyChanged(nameof(RegisterName));
            }
        }
        public string RegisterPassword
        {
            get => registerPassword;
            set
            {
                registerPassword = value;
                OnPropertyChanged(nameof(RegisterPassword));
            }
        }
        public string RegisterPasswordVerification
        {
            get => registerPasswordVerification;
            set
            {
                registerPasswordVerification = value;
                OnPropertyChanged(nameof(RegisterPasswordVerification));
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
        public ICommand UserLogin => new RelayCommand(Login, CanLogin);
        public ICommand UserLogout => new RelayCommand(Logout, CanLogout);
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
            return LoginName != string.Empty && LoginPassword != string.Empty && LoggedUser == null;
        }

        private void Login(object? obj)
        {
            foreach (User user in Users)
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
        }
    }
}
