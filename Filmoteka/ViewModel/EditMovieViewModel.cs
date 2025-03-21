using Filmoteka.Framework;
using Filmoteka.Model;
using Filmoteka.View.UserControls;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    internal class EditMovieViewModel : UserMovieViewmodel
    {
        private string? editMode;
        private string? editedName;
        private string? editedYear;
        private GenreType? editedGenre;
        private string? editedDescription;
        private string? editedPicturePath;
        private bool? delete;
        private string? message;
        public string? EditMode
        {
            get => editMode;
            set
            {
                editMode = value;
                OnPropertyChanged(nameof(EditMode));
                if (editMode != null)
                {
                    switch(editMode)
                    {
                        case "Name": EditedName = "";break;
                        case "Genre": EditedGenre = GenreType.Akční;break;
                        case "Description": EditedDescription = ""; break;
                        case "Picture": EditedPicturePath = "Cesta k obrázku"; break;
                        case "Delete": Delete = true; break;
                    }
                }
                else
                {
                    EditedName = null;
                    EditedGenre = null;
                    EditedDescription = null;
                    EditedPicturePath = null;
                    Delete = null;
                }
            }
        }
        public string? EditedName 
        {
            get => editedName;
            set 
            {
                editedName = value;
                OnPropertyChanged(nameof(EditedName));
                if (_errors.ContainsKey(nameof(EditedName)))
                {
                    CheckErrors(nameof(EditedName));
                    OnErrorsChanged(nameof(EditedName));
                }
            } 
        }
        public string? EditedYear
        {
            get => editedYear;
            set
            {
                editedYear = value;
                OnPropertyChanged(nameof(EditedYear));
                if (_errors.ContainsKey(nameof(EditedYear)))
                {
                    CheckErrors(nameof(EditedYear));
                    OnErrorsChanged(nameof(EditedYear));
                }
            }
        }
        public GenreType? EditedGenre
        {
            get => editedGenre;
            set
            {
                editedGenre = value;
                OnPropertyChanged(nameof(EditedGenre));
            }
        }
        public string? EditedDescription
        {
            get => editedDescription;
            set
            {
                editedDescription = value;
                OnPropertyChanged(nameof(EditedDescription));
            }
        }
        public string? EditedPicturePath
        {
            get => editedPicturePath;
            set
            {
                editedPicturePath = value;
                OnPropertyChanged(nameof(EditedPicturePath));
            }
        }
        public bool? Delete
        {
            get => delete;
            set
            {
                delete = value;
                OnPropertyChanged(nameof(Delete));
            }
        }
        public string? Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public ICommand EditModeClose => new RelayCommand(CloseEdit);
        public ICommand NameEdit => new RelayCommand(EditName);
        public ICommand GenreEdit => new RelayCommand(EditGenre);
        public ICommand OriginalDescription => new RelayCommand(ShowOriginalDescription);
        public ICommand DescriptionEdit => new RelayCommand(EditDescription);
        public ICommand PictureEdit => new RelayCommand(EditPicture);
        public ICommand MovieDelete => new RelayCommand(DeleteMovie, CanDeleteMovie);
        public EditMovieViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
            userCollectionViewModel.PropertyChanged += UserCollectionViewModelEditMode_PropertyChanged;
        }
        private void UserCollectionViewModelEditMode_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (userCollectionViewModel.EditMode != EditMode)
            {
                EditMode = userCollectionViewModel.EditMode;
            }
        }
        private void EditName(object? obj)
        {
            CheckErrors(nameof(EditedName));
            CheckErrors(nameof(EditedYear));
            if (!HasErrors)
            {
                MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                editedMovie.Name = EditedName;
                editedMovie.Year = Convert.ToInt32(EditedYear);
                movieCollectionViewModel.SelectedMovie = editedMovie;
                movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Name = EditedName;
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Year = Convert.ToInt32(EditedYear);
                    mc.SaveChanges();
                };
                userCollectionViewModel.Users.Clear();
                userCollectionViewModel.GetUsersFromDatabase();
            }
        }
        private void EditGenre(object? obj)
        {
            MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
            movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
            editedMovie.Genre = (GenreType)EditedGenre;
            movieCollectionViewModel.SelectedMovie = editedMovie;
            movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
            using (MovieContext mc = new MovieContext())
            {
                mc.Movies.Where(x => x.Id == editedMovie.Id).First().Genre = (GenreType)EditedGenre;
                mc.SaveChanges();
            }
        }
        private void ShowOriginalDescription(object? obj)
        {
            EditedDescription = movieCollectionViewModel.SelectedMovie.Description;
        }
        private void EditDescription(object? obj)
        {
            CheckErrors(nameof(EditedDescription));
            if (!HasErrors)
            {
                MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                editedMovie.Description = EditedDescription;
                movieCollectionViewModel.SelectedMovie = editedMovie;
                movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Description = EditedDescription;
                    mc.SaveChanges();
                };
            }
        }
        private void EditPicture(object? obj)
        {
            string targetPath = string.Empty;
            try
            {
                CreateDirectoryIfNotExist();
                string pictureFileName = Path.GetFileName(EditedPicturePath);
                pictureFileName = CheckFileNameUniqueness(pictureFileName);
                targetPath = CopyPictureToPostersFolder(pictureFileName);
                MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                editedMovie.PicturePath = EditedPicturePath;
                movieCollectionViewModel.SelectedMovie = editedMovie;
                movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().PicturePath = EditedPicturePath;
                    mc.SaveChanges();
                };
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        private void CreateDirectoryIfNotExist()
        {
            try
            {
                Directory.CreateDirectory("Posters");
            }
            catch (Exception)
            {
                throw new Exception("Nelze vytvořit složku obrázků");
            }
        }
        private string CheckFileNameUniqueness(string pictureFileName)
        {
            try
            {
                string targetFileName = Path.GetFileName(EditedPicturePath);
                string[] pictureFilePaths = Directory.GetFiles("Posters");
                List<string> pictureFileNames = new List<string>();
                foreach (var filePath in pictureFilePaths)
                {
                    pictureFileNames.Add(Path.GetFileName(filePath));
                }
                while (pictureFileNames.Contains(targetFileName))
                {
                    string extension = Path.GetExtension(targetFileName);
                    string newFileName = Path.GetFileNameWithoutExtension(targetFileName) + "x";
                    targetFileName = newFileName + extension;
                }
                return targetFileName;
            }
            catch (Exception)
            {
                throw new Exception("Nelze vytvořit jedinečný název souboru");
            }

        }
        private string CopyPictureToPostersFolder(string pictureFileName)
        {
            try
            {
                string targetPath = Path.Combine("Posters", pictureFileName);
                File.Copy(EditedPicturePath, targetPath);
                return targetPath;
            }
            catch (Exception)
            {
                throw new Exception("Nelze zkopírovat obrázek do složky");
            }
        }
        private bool CanDeleteMovie(object? arg)
        {
            return movieCollectionViewModel.SelectedMovie != null;
        }
        private void DeleteMovie(object? obj)
        {           
            using (MovieContext mc = new MovieContext())
            {               
                foreach (UserMovie userMovie in movieCollectionViewModel.SelectedMovie.Ratings)
                {
                    mc.UserMovies.Remove(userMovie);
                }
                Movie deleteDatabaseMovie = mc.Movies.Where(x => x.Id == movieCollectionViewModel.SelectedMovie.Id).First();
                mc.Movies.Remove(deleteDatabaseMovie);
                mc.SaveChanges();
            }
            userCollectionViewModel.Users.Clear();
            userCollectionViewModel.GetUsersFromDatabase();
            MovieViewModel deleteMovie = movieCollectionViewModel.SelectedMovie;
            movieCollectionViewModel.Movies.Remove(deleteMovie);
        }
        private void CheckErrors(string propertyName)
        {
            RemoveErrors(propertyName);
            switch (propertyName)
            {
                case nameof(EditedName):
                    if (string.IsNullOrWhiteSpace(EditedName))
                        AddError(propertyName, "Zadej název filmu");
                    if (movieCollectionViewModel.Movies.Any(x => x.Name == EditedName && x.Year == Convert.ToInt32(EditedYear)))
                        AddError(propertyName, "Film s tímto názvem a rokem výroby je již v seznamu"); break;
                case nameof(EditedDescription):
                    if (string.IsNullOrWhiteSpace(EditedDescription))
                        AddError(propertyName, "Zadej popis filmu"); break;
                case nameof(EditedYear):
                    if (EditedYear == "")
                        AddError(propertyName, "Zadej rok výroby filmu");
                    else if (Convert.ToInt32(EditedYear) < 1900 || Convert.ToInt32(EditedYear) > DateTime.Now.Year)
                        AddError(propertyName, "Rok výroby mimo rozsah (1900 - letošní rok)"); break;
            }
        }
        private void CloseEdit(object? obj)
        {
            _errors.Clear();
            EditMode = null;
            userCollectionViewModel.EditMode = null;
        }
    }
}
