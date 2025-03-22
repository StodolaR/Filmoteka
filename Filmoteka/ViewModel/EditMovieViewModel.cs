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
    internal class EditMovieViewModel : MovieOperationViewModel
    {
        private string? editMode;
        private bool? delete;
        public string? EditMode
        {
            get => editMode;
            set
            {
                editMode = value;
                OnPropertyChanged(nameof(EditMode));
                if (editMode != null)
                {
                    NewMovieName = null;
                    NewMovieGenre = null;
                    NewMovieDescription = null;
                    NewMoviePicturePath = null;
                    Delete = null;
                    switch (editMode)
                    {
                        case "Name": NewMovieName = "";break;
                        case "Genre": NewMovieGenre = GenreType.Akční;break;
                        case "Description": NewMovieDescription = ""; break;
                        case "Picture": NewMoviePicturePath = "Cesta k obrázku"; break;
                        case "Delete": Delete = true; break;
                    }
                }
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
            CheckErrors(nameof(NewMovieName));
            CheckErrors(nameof(NewMovieYear));
            if (!HasErrors)
            {
                MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                editedMovie.Name = NewMovieName;
                editedMovie.Year = Convert.ToInt32(NewMovieYear);
                movieCollectionViewModel.SelectedMovie = editedMovie;
                movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Name = NewMovieName;
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Year = Convert.ToInt32(NewMovieYear);
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
            editedMovie.Genre = (GenreType)NewMovieGenre;
            movieCollectionViewModel.SelectedMovie = editedMovie;
            movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
            using (MovieContext mc = new MovieContext())
            {
                mc.Movies.Where(x => x.Id == editedMovie.Id).First().Genre = (GenreType)NewMovieGenre;
                mc.SaveChanges();
            }
        }
        private void ShowOriginalDescription(object? obj)
        {
            NewMovieDescription = movieCollectionViewModel.SelectedMovie.Description;
        }
        private void EditDescription(object? obj)
        {
            CheckErrors(nameof(NewMovieDescription));
            if (!HasErrors)
            {
                MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                editedMovie.Description = NewMovieDescription;
                movieCollectionViewModel.SelectedMovie = editedMovie;
                movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Where(x => x.Id == editedMovie.Id).First().Description = NewMovieDescription;
                    mc.SaveChanges();
                };
            }
        }
        private void EditPicture(object? obj)
        {
            string targetPath = string.Empty;
            try
            {
                if (NewMoviePicturePath != "Cesta k obrázku")
                {
                    CreateDirectoryIfNotExist();
                    string pictureFileName = CheckFileNameUniqueness();
                    targetPath = CopyPictureToPostersFolder(pictureFileName);
                    MovieViewModel editedMovie = movieCollectionViewModel.SelectedMovie;
                    movieCollectionViewModel.Movies.Remove(movieCollectionViewModel.SelectedMovie);
                    editedMovie.PicturePath = targetPath;
                    movieCollectionViewModel.SelectedMovie = editedMovie;
                    movieCollectionViewModel.Movies.Add(movieCollectionViewModel.SelectedMovie);
                    using (MovieContext mc = new MovieContext())
                    {
                        mc.Movies.Where(x => x.Id == editedMovie.Id).First().PicturePath = targetPath;
                        mc.SaveChanges();
                    }
                    ;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
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
        private void CloseEdit(object? obj)
        {
            _errors.Clear();
            EditMode = null;
            userCollectionViewModel.EditMode = null;
        }
    }
}
