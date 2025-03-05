using Filmoteka.Framework;
using Filmoteka.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    public class ZebricekViewModel : ViewModelBase
    {
        private UzivatelViewModel uzivatelViewModel;
        private string newMovieName = string.Empty;
        private GenreType newMovieGenre;
        private string newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string newMoviePicturePath = "Cesta k obrázku";
        private string message = string.Empty;
        private Movie? selectedMovie;
        private int newMovieRating;
        private string newMovieReview = string.Empty;
        private User? loggedUser;

        public User? LoggedUser
        {
            get => loggedUser;
            set
            {
                loggedUser = value;
                OnPropertyChanged(nameof(IsUserLogged));
            }
        }
        public bool IsUserLogged
        {
            get { return LoggedUser != null; }
        }
        
        public ObservableCollection<Movie> Movies { get; set; }
        public Movie? SelectedMovie
        {
            get => selectedMovie;
            set
            {
                if (selectedMovie == value) return;
                selectedMovie = value;
                if (SelectedMovie !=null)
                {
                    AddRatingsToCollection();
                }
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }
        public ObservableCollection<UserMovie> SelectedMovieRatings { get; set; }
        public int SelectedMovieAvgRating
        {
            get
            {
                if (SelectedMovieRatings.Count > 0)
                {
                    return Convert.ToInt32(SelectedMovieRatings.Average(x => x.Rating) * 20);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string NewMovieName
        {
            get => newMovieName;
            set
            {
                newMovieName = value;
                OnPropertyChanged(nameof(NewMovieName));
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
            }
        }
        public string NewMovieYear
        {
            get => newMovieYear;
            set
            {
                newMovieYear = value;
                OnPropertyChanged(nameof(NewMovieYear));
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
        public string NewMovieReview
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
        public ICommand AddNewRating => new RelayCommand(AddRating, CanAddRating);

        

        public ZebricekViewModel(UzivatelViewModel uzivatelViewModel)
        {
            this.uzivatelViewModel = uzivatelViewModel;
            this.uzivatelViewModel.PropertyChanged += UzivatelViewModel_PropertyChanged;
            SelectedMovieRatings = new ObservableCollection<UserMovie>();
            Movies = new ObservableCollection<Movie>();
            using (MovieContext mc = new MovieContext())
            {
                foreach (Movie movie in mc.Movies.Include(x => x.UserMovies))
                {
                    Movies.Add(movie);
                }
            }
        }

        private void UzivatelViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (LoggedUser != uzivatelViewModel.LoggedUser)
                LoggedUser = uzivatelViewModel.LoggedUser;
        }

        private bool CanAddMovie(object? arg)
        {
            return NewMovieName != string.Empty && NewMovieDescription != string.Empty && NewMovieYear != string.Empty;
        }

        private void AddMovie(object? obj)
        {
            using (MovieContext mc = new MovieContext())
            {
                Movie newMovie = CreateNewMovieWithRating();
                mc.Movies.Add(newMovie);
                mc.SaveChanges();
                var newMovieFromDatabase = mc.Movies.OrderBy(x => x.Id).Last();
                AddMovieToCollection(newMovieFromDatabase);
            }
        }
        private Movie CreateNewMovieWithRating()
        {
            if (NewMoviePicturePath != "Cesta k obrázku")
            {
                try
                {
                    string targetPath = CopyPictureToPostersFolder();
                    Movie newMovieWithPicture = new Movie{Name = NewMovieName, Genre = NewMovieGenre,
                        Description = NewMovieDescription, Year = Convert.ToInt32(NewMovieYear), PicturePath = targetPath};
                    UserMovie newRatingPictureMovie = new UserMovie { Movie = newMovieWithPicture, UserId = LoggedUser.Id,
                        Rating = NewMovieRating, Review = NewMovieReview};
                    newMovieWithPicture.UserMovies.Add(newRatingPictureMovie);
                    return newMovieWithPicture;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            Movie newMovie = new Movie{Name = NewMovieName, Genre = NewMovieGenre,
                Description = NewMovieDescription, Year = Convert.ToInt32(NewMovieYear)};
            UserMovie newRating = new UserMovie { Movie = newMovie, UserId =LoggedUser.Id, 
                Rating = NewMovieRating, Review = NewMovieReview};
            newMovie.UserMovies.Add(newRating);
            return newMovie;
        }
        private string CopyPictureToPostersFolder()
        {
            string pictureFileName = Path.GetFileName(NewMoviePicturePath);
            pictureFileName = CheckFileNameUniqueness(pictureFileName);
            try
            {
                string targetPath = Path.Combine("Posters", pictureFileName);
                File.Copy(NewMoviePicturePath, targetPath);
                return targetPath;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Nelze vytvořit složku obrázků" || ex.Message == "Nelze vytvořit jedinečný název souboru")
                {
                    throw;
                }
                else
                {
                    throw new Exception("Nelze zkopírovat obrázek do složky");
                }
            }
        }

        private string CheckFileNameUniqueness(string pictureFileName)
        {
            try
            {
                if (!Directory.Exists("Posters"))
                {
                    CreateDirectoryIfNotExist();
                    return pictureFileName;
                }
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
            catch (Exception ex)
            {
                if (ex.Message == "Nelze vytvořit složku obrázků") throw;
                else
                    throw new Exception("Nelze vytvořit jedinečný název souboru");
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
        private void AddMovieToCollection(Movie newMovieFromDatabase)
        {
            if (newMovieFromDatabase.Name != newMovieName)
            {
                ResetProperties();
                Message = "Nelze se připojit k databázi, film nepřidán";
            }
            else
            {
                Movies.Add(newMovieFromDatabase);
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
        private bool CanAddRating(object? arg)
        {
            return true;
        }

        private void AddRating(object? obj)
        {
            using (MovieContext mc = new MovieContext())
            {
                foreach (UserMovie rating in SelectedMovie.UserMovies)
                {
                    if (rating.UserId == LoggedUser.Id)
                    {
                        rating.Rating = NewMovieRating;
                        rating.Review = NewMovieReview;
                        mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == SelectedMovie.Id).First().Rating = NewMovieRating;
                        mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == SelectedMovie.Id).First().Review = NewMovieReview;
                        mc.SaveChanges();
                        AddRatingsToCollection();
                        return;
                    }
                }
                UserMovie newRating = new UserMovie { MovieId = SelectedMovie.Id, UserId = LoggedUser.Id, 
                    Rating = NewMovieRating, Review = NewMovieReview};
                SelectedMovie.UserMovies.Add(newRating);
                mc.UserMovies.Add(newRating);
                mc.SaveChanges();
                AddRatingsToCollection();
            }
        }
        private void AddRatingsToCollection()
        {
            SelectedMovieRatings.Clear();
            foreach (UserMovie rating in SelectedMovie.UserMovies)
            {
                SelectedMovieRatings.Add(rating);
            }
            OnPropertyChanged(nameof(SelectedMovieAvgRating));
            ResetProperties();
        }
    }
}
