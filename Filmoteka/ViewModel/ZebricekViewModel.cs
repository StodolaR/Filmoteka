using Filmoteka.Framework;
using Filmoteka.Model;
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
    public class ZebricekViewModel : ViewModelBase
    {
        private string newMovieName = string.Empty;
        private GenreType newMovieGenre;
        private string newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string newMoviePicturePath = "Cesta k obrázku";
        private string message = string.Empty;
        private Movie? selectedMovie;
        private int newMovieRating;
        private string newMovieReview = string.Empty;

        public ObservableCollection<Movie> Movies { get; set; }
        public Movie? SelectedMovie
        {
            get => selectedMovie;
            set 
            {
                selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
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
        public ZebricekViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            using (MovieContext mc = new MovieContext())
            {
                foreach (Movie movie in mc.Movies)
                {
                    Movies.Add(movie);
                }
            }
        }
        private bool CanAddMovie(object? arg)
        {
            return NewMovieName != string.Empty &&  NewMovieDescription != string.Empty && newMovieYear != string.Empty;
        }

        private void AddMovie(object? obj)
        {
            using(MovieContext mc = new MovieContext())
            {
                if (NewMoviePicturePath == "Cesta k obrázku")
                {
                    mc.Movies.Add(new Movie { Name = NewMovieName, Genre = NewMovieGenre, Description = NewMovieDescription,
                                                Year = Convert.ToInt32(NewMovieYear)});
                    mc.SaveChanges();
                    var newMovie = mc.Movies.OrderBy(x => x.Id).Last();
                    AddMovieToCollection(newMovie);
                }
                else
                {
                    string pictureFileName = Path.GetFileName(NewMoviePicturePath);
                    pictureFileName = CheckFileNameUniqueness(pictureFileName);
                    if (pictureFileName == string.Empty) return;
                    string targetPath = Path.Combine("Posters", pictureFileName);
                    File.Copy(NewMoviePicturePath, targetPath);
                    mc.Movies.Add(new Movie { Name = NewMovieName, Genre = NewMovieGenre, Description = NewMovieDescription,
                                                Year = Convert.ToInt32(NewMovieYear), PicturePath = targetPath });
                    mc.SaveChanges();
                    var newMovie = mc.Movies.OrderBy(x => x.Id).Last();
                    AddMovieToCollection(newMovie);
                }
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
                Movies.Add(newMovie);
                ResetProperties();
                Message = "Film úspěšně přidán";
            }
        }
        private string CheckFileNameUniqueness(string pictureFileName)
        {
            if (!Directory.Exists("Posters"))
            {
                CreateDirectoryIfNotExist();
                return pictureFileName;
            }
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
            catch (Exception ex)
            {
                Message = "Nelze vytvořit jedinečný název souboru, nelze přidávat obrázky k filmům"
                    + Environment.NewLine + ex.Message;
                return string.Empty;
            }
        }
        private void CreateDirectoryIfNotExist()
        {
            try
            {
                Directory.CreateDirectory("Posters");
                return;
            }
            catch (Exception ex)
            {
                Message = "Nelze vytvořit složku obrázků, nelze přidávat obrázky k filmům"
                    + Environment.NewLine + ex.Message;
            }
        }
        private void ResetProperties()
        {
            NewMovieName = string.Empty;
            NewMovieGenre = 0;
            NewMovieDescription = string.Empty;
            NewMovieYear = string.Empty;
            NewMoviePicturePath = "Cesta k obrázku";
        }
    }
}
