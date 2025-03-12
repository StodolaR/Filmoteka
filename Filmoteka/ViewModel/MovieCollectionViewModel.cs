using Filmoteka.Framework;
using Filmoteka.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;

namespace Filmoteka.ViewModel
{
    public class MovieCollectionViewModel : ViewModelBase
    {
        private string newMovieName = string.Empty;
        private GenreType newMovieGenre;
        private string newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string newMoviePicturePath = "Cesta k obrázku";
        private string message = string.Empty;
        private MovieViewModel? selectedMovie;
        private int newMovieRating;
        private string? newMovieReview;
        private UserViewModel? loggedUser;
        private MovieViewModel selectedSearchedMovie;

        public UserViewModel? LoggedUser
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
        
        public ObservableCollection<MovieViewModel> Movies { get; set; }
        public MovieViewModel? SelectedMovie
        {
            get => selectedMovie;
            set
            {
                selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }
        public MovieViewModel SelectedSearchedMovie 
        {
            get => selectedSearchedMovie;
            set 
            {
                selectedSearchedMovie = value;
                if (value != null)
                {
                    SelectedMovie = value;
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

        

        public MovieCollectionViewModel()
        {
            Movies = new ObservableCollection<MovieViewModel>();
            using (MovieContext mc = new MovieContext())
            {
                foreach (Movie movie in mc.Movies.Include(x => x.UserMovies))
                {
                    ObservableCollection<UserMovie> ratings = new ObservableCollection<UserMovie>();
                    foreach (UserMovie rating in movie.UserMovies)
                    {
                        ratings.Add(rating);
                    }
                    int avgRating = (int)(ratings.Average(x => x.Rating)*20);
                    MovieViewModel movieForViewModel = new MovieViewModel {Id = movie.Id, AvgRating = avgRating, Description = movie.Description,
                     Genre = movie.Genre, Name = movie.Name, PicturePath = movie.PicturePath, Ratings = ratings, Year = movie.Year};
                    Movies.Add(movieForViewModel);
                }
            }
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
                Movie newMovieFromDatabase = mc.Movies.OrderBy(x => x.Id).Last();
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
                Movies.Add(new MovieViewModel {Id = newMovie.Id, AvgRating = newMovie.UserMovies.First().Rating *20, 
                    Description = newMovie.Description, Genre = newMovie.Genre, Name = newMovie.Name, PicturePath = newMovie.PicturePath, 
                    Ratings = newRatings , Year = newMovie.Year});
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
                MovieViewModel movieWithNewRating = SelectedMovie;
                Movies.Remove(SelectedMovie);
                foreach (UserMovie rating in movieWithNewRating.Ratings)
                {
                    if (rating.UserId == LoggedUser.Id)
                    {
                        rating.Rating = NewMovieRating;
                        rating.Review = NewMovieReview;
                        movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                        SelectedMovie = movieWithNewRating;
                        Movies.Add(SelectedMovie);
                        mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == SelectedMovie.Id).First().Rating = NewMovieRating;
                        mc.UserMovies.Where(x => x.UserId == LoggedUser.Id && x.MovieId == SelectedMovie.Id).First().Review = NewMovieReview;
                        mc.SaveChanges();
                        ResetProperties();
                        return;
                    }
                }
                UserMovie newRating = new UserMovie { MovieId = movieWithNewRating.Id, UserId = LoggedUser.Id, 
                    Rating = NewMovieRating, Review = NewMovieReview};
                movieWithNewRating.Ratings.Add(newRating);
                movieWithNewRating.AvgRating = (int)(movieWithNewRating.Ratings.Average(x => x.Rating) * 20);
                SelectedMovie = movieWithNewRating;
                Movies.Add(SelectedMovie);
                mc.UserMovies.Add(newRating);
                mc.SaveChanges();
                ResetProperties();
            }
        }
    }
}
