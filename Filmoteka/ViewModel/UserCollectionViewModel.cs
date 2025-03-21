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
        private string? editMode;

        public ObservableCollection<UserViewModel> Users { get; set; }
        public string? EditMode 
        {
            get => editMode;
            set 
            {
                editMode = value;
                OnPropertyChanged(nameof(EditMode));
                if (editMode == null)
                {
                    Message = "";
                }
            } 
        }
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
                if (_errors.ContainsKey(nameof(LoginName)))
                {
                    CheckErrors(nameof(LoginName));
                    OnErrorsChanged(nameof(LoginName));
                }
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
                if (_errors.ContainsKey(nameof(RegistrationName)))
                {
                    CheckErrors(nameof(RegistrationName));
                    OnErrorsChanged(nameof(RegistrationName));
                }
            }
        }
        public string RegistrationPassword
        {
            get => registrationPassword;
            set
            {
                registrationPassword = value;
                OnPropertyChanged(nameof(RegistrationPassword));
                if (_errors.ContainsKey(nameof(RegistrationPassword)))
                {
                    CheckErrors(nameof(RegistrationPassword));
                    OnErrorsChanged(nameof(RegistrationPassword));
                }
            }
        }
        public string RegistrationPasswordVerification
        {
            get => registrationPasswordVerification;
            set
            {
                registrationPasswordVerification = value;
                OnPropertyChanged(nameof(RegistrationPasswordVerification));
                if (_errors.ContainsKey(nameof(RegistrationPasswordVerification)))
                {
                    CheckErrors(nameof(RegistrationPasswordVerification));
                    OnErrorsChanged(nameof(RegistrationPasswordVerification));
                }
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
        public ICommand ErrorsReset => new RelayCommand(ResetErrors);
        public UserCollectionViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            GetUsersFromDatabase();
        }
        public void GetUsersFromDatabase()
        {
            using (MovieContext mc = new MovieContext())
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
            return LoginName != string.Empty && LoginPassword != string.Empty && LoggedUser == null && EditMode == null;
        }
        private void Login(object? obj)
        {
            if (EditLogin())
            {
                Message = "Editace filmu: " + EditMode;
                LoginName = string.Empty;
                LoginPassword = string.Empty;
                return;
            }
            CheckErrors(nameof(LoginName));
            if (!HasErrors)
            {
                if (Users.Any(x => x.Name == LoginName && x.Password == LoginPassword))
                {
                    LoggedUser = Users.Where(x => x.Name == loginName).First();
                    Message = "Přihlášen: " + LoginName;
                    LoginName = string.Empty;
                    LoginPassword = string.Empty;
                }
                else
                {
                    Message = "Nesprávné heslo";
                    LoginPassword = string.Empty;
                }
            }
        }
        private bool EditLogin()
        {
            if (loginName == "Movie")
            {
                switch (LoginPassword)
                {
                    case "Name": EditMode = "Name"; return true;
                    case "Genre": EditMode = "Genre"; return true;
                    case "Description": EditMode = "Description"; return true;
                    case "Picture": EditMode = "Picture"; return true;
                    case "Delete": EditMode = "Delete"; return true;
                }
            }
            return false;
        }
        private bool CanLogout(object? arg)
        {
            return LoggedUser != null || EditMode != null;
        }
        private void Logout(object? obj)
        {
            LoggedUser = null;
            EditMode = null;
            Message = string.Empty;
            RegistrationMessage = string.Empty;
        }
        private bool CanRegister(object? arg)
        {
            return RegistrationName != string.Empty && RegistrationPassword != string.Empty && RegistrationPasswordVerification != string.Empty;
        }
        private void Register(object? obj)
        {
            CheckErrors(nameof(RegistrationName));
            CheckErrors(nameof(RegistrationPassword));
            CheckErrors(nameof(RegistrationPasswordVerification));
            if (!HasErrors)
            {
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
                        UserViewModel newUserViewModel = new UserViewModel{Id = newUser.Id, Name = newUser.Name,
                            Password = newUser.Password, Ratings = new ObservableCollection<UserMovie>()};
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
        private void CheckErrors(string propertyName)
        {
            RemoveErrors(propertyName);
            switch (propertyName)
            {
                case nameof(LoginName): 
                    if (!Users.Any(x => x.Name == LoginName)) 
                        AddError(propertyName, "Tento uživatel není registrován"); break;
                case nameof(LoginPassword):
                    if (Users.Any(x => x.Name == LoginName && x.Password != LoginPassword))
                        AddError(propertyName, "Chybné heslo"); break;
                case nameof(RegistrationName):
                    if (Users.Any(x => x.Name == RegistrationName))
                        AddError(propertyName, "Toto jméno již používá jiný uživatel");
                    if (string.IsNullOrEmpty(RegistrationName) || RegistrationName.Length < 3)
                        AddError(propertyName, "Uživatelské jméno musí mít alespoň 3 znaky"); break;
                case nameof(RegistrationPassword):
                    if (RegistrationPassword.Length < 3)
                        AddError(propertyName, "Heslo musí mít alespoň 3 znaky"); break;
                case nameof(RegistrationPasswordVerification):
                    if (RegistrationPasswordVerification != RegistrationPassword)
                        AddError(propertyName, "Heslo nesouhlasí, zkuste zadat znova"); break;
            }
        }
        private void ResetErrors(object? obj)
        {
            _errors.Clear();
            OnErrorsChanged(nameof(LoginName));
            OnErrorsChanged(nameof(RegistrationName));
            OnErrorsChanged(nameof(RegistrationPassword));
            OnErrorsChanged(nameof(RegistrationPasswordVerification));
            if (Message == "Nesprávné heslo")
            {
                Message = "";
            }
        }
    }
}
