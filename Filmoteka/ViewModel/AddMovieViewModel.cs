using Filmoteka.Framework;
using Filmoteka.Model;
using Filmoteka.View.UserControls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    class AddMovieViewModel : UserMovieViewmodel
    {
        private string newMovieName = string.Empty;
        private GenreType newMovieGenre;
        private string newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string newMoviePicturePath = "Cesta k obrázku";
        private string message = string.Empty;
        private int newMovieRating;
        private string? newMovieReview;
        public string NewMovieName
        {
            get => newMovieName;
            set
            {
                newMovieName = value;
                OnPropertyChanged(nameof(NewMovieName));
                if (_errors.ContainsKey(nameof(NewMovieName)))
                {
                    CheckErrors(nameof(NewMovieName));
                    OnErrorsChanged(nameof(NewMovieName));
                }
            }
        }
        public GenreType NewMovieGenre
        {
            get => newMovieGenre;
            set
            {
                newMovieGenre = value;
                OnPropertyChanged(nameof(newMovieGenre));
            }
        }
        public string NewMovieDescription
        {
            get => newMovieDescription;
            set
            {
                newMovieDescription = value;
                OnPropertyChanged(nameof(NewMovieDescription));
                if (_errors.ContainsKey(nameof(NewMovieDescription)))
                {
                    CheckErrors(nameof(NewMovieDescription));
                    OnErrorsChanged(nameof(NewMovieDescription));
                }
            }
        }
        public string NewMovieYear
        {
            get => newMovieYear;
            set
            {
                newMovieYear = value;
                OnPropertyChanged(nameof(NewMovieYear));
                if (_errors.ContainsKey(nameof(NewMovieYear)))
                {
                    CheckErrors(nameof(NewMovieYear));
                    OnErrorsChanged(nameof(NewMovieYear));
                }
            }
        }
        public string NewMoviePicturePath
        {
            get => newMoviePicturePath;
            set
            {
                newMoviePicturePath = value;
                OnPropertyChanged(nameof(NewMoviePicturePath));
            }
        }
        public int NewMovieRating
        {
            get => newMovieRating;
            set
            {
                newMovieRating = value;
                OnPropertyChanged(nameof(NewMovieRating));
            }
        }
        public string? NewMovieReview
        {
            get => newMovieReview;
            set
            {
                newMovieReview = value;
                OnPropertyChanged(nameof(NewMovieReview));
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
        public ICommand AddNewMovie => new RelayCommand(AddMovie, CanAddMovie);
        public ICommand ErrorsReset => new RelayCommand(ResetErrors);
        public AddMovieViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
        }
        private bool CanAddMovie(object? arg)
        {
            return NewMovieName != string.Empty && NewMovieDescription != string.Empty && NewMovieYear != string.Empty;
        }
        private void AddMovie(object? obj)
        {
            CheckErrors(nameof(NewMovieName));
            CheckErrors(nameof(NewMovieDescription));
            CheckErrors(nameof(NewMovieYear));
            if (!HasErrors)
            {
                string targetPath = string.Empty;
                try
                {
                    CreateDirectoryIfNotExist();
                    string pictureFileName = Path.GetFileName(NewMoviePicturePath);
                    pictureFileName = CheckFileNameUniqueness(pictureFileName);
                    targetPath = CopyPictureToPostersFolder(pictureFileName);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                Movie newMovie = CreateNewMovieWithRating(targetPath);
                using (MovieContext mc = new MovieContext())
                {
                    mc.Movies.Add(newMovie);
                    mc.SaveChanges();
                    Movie newMovieFromDatabase = (mc.Movies.Include(y => y.UserMovies).ThenInclude(z => z.User)).OrderBy(x => x.Id).Last();
                    AddMovieToCollection(newMovieFromDatabase);
                }
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
                string targetFileName = Path.GetFileName(NewMoviePicturePath);
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
                File.Copy(NewMoviePicturePath, targetPath);
                return targetPath;
            }
            catch (Exception)
            {               
                throw new Exception("Nelze zkopírovat obrázek do složky");
            }
        }
        private Movie CreateNewMovieWithRating(string targetPath)
        {
            if (targetPath != string.Empty)
            {
                Movie newMovieWithPicture = new Movie{Name = NewMovieName, Genre = NewMovieGenre, Description = NewMovieDescription,
                    Year = Convert.ToInt32(NewMovieYear), PicturePath = targetPath};
                UserMovie newRatingPictureMovie = new UserMovie{Movie = newMovieWithPicture, UserId = LoggedUser.Id, 
                    Rating = NewMovieRating,Review = NewMovieReview};
                newMovieWithPicture.UserMovies.Add(newRatingPictureMovie);
                return newMovieWithPicture;
            }
            Movie newMovie = new Movie{Name = NewMovieName, Genre = NewMovieGenre, Description = NewMovieDescription, 
                Year = Convert.ToInt32(NewMovieYear)};
            UserMovie newRating = new UserMovie{Movie = newMovie, UserId = LoggedUser.Id, Rating = NewMovieRating, Review = NewMovieReview};
            newMovie.UserMovies.Add(newRating);
            return newMovie;
        }
        private void AddMovieToCollection(Movie newMovie)
        {
            if (newMovie.Name != newMovieName)
            {
                ResetProperties();
                Message = "Nelze se připojit k databázi, film nepřidán";
            }
            else
            {
                ObservableCollection<UserMovie> newRatings = new ObservableCollection<UserMovie>();
                newRatings.Add(newMovie.UserMovies.First());
                movieCollectionViewModel.Movies.Add(new MovieViewModel{Id = newMovie.Id, AvgRating = newMovie.UserMovies.First().Rating * 20,
                    Description = newMovie.Description, Genre = newMovie.Genre, Name = newMovie.Name, PicturePath = newMovie.PicturePath,
                    Ratings = newRatings, Year = newMovie.Year});
                ResetProperties();
                Message = "Film úspěšně přidán";
            }
        }
        private void ResetProperties()
        {
            NewMovieName = string.Empty;
            NewMovieGenre = 0;
            NewMovieDescription = string.Empty;
            NewMovieYear = string.Empty;
            NewMoviePicturePath = "Cesta k obrázku";
            NewMovieReview = string.Empty;
            NewMovieRating = 0;
        }
        private void CheckErrors(string propertyName)
        {
            RemoveErrors(propertyName);
            switch (propertyName)
            {
                case nameof(NewMovieName):
                    if (string.IsNullOrWhiteSpace(NewMovieName))
                        AddError(propertyName, "Zadej název filmu");
                    if (movieCollectionViewModel.Movies.Any(x => x.Name == NewMovieName && x.Year == Convert.ToInt32(NewMovieYear)))
                        AddError(propertyName, "Film s tímto názvem a rokem výroby je již v seznamu"); break;
                case nameof(NewMovieDescription):
                    if (string.IsNullOrWhiteSpace(NewMovieDescription))
                        AddError(propertyName, "Zadej popis filmu"); break;
                case nameof(NewMovieYear):
                    if (NewMovieYear == "")
                        AddError(propertyName, "Zadej rok výroby filmu");
                    else if (Convert.ToInt32(NewMovieYear) < 1900 || Convert.ToInt32(NewMovieYear) > DateTime.Now.Year)
                        AddError(propertyName, "Rok výroby mimo rozsah (1900 - letošní rok)");break;
            }
        }
        private void ResetErrors(object? obj)
        {
            _errors.Clear();
            OnErrorsChanged(nameof(NewMovieName));
            OnErrorsChanged(nameof(NewMovieDescription));
            OnErrorsChanged(nameof(NewMovieYear));
        }
    }
}
