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
    class AddMovieViewModel : MovieOperationViewModel
    {        
        private int newMovieRating;
        private string? newMovieReview;
        
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
        
        public ICommand AddNewMovie => new RelayCommand(AddMovie, CanAddMovie);
        public ICommand FormReset => new RelayCommand(ResetForm);
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
                    if(NewMoviePicturePath != "Cesta k obrázku")
                    {
                        CreateDirectoryIfNotExist();
                        string pictureFileName = CheckFileNameUniqueness();
                        targetPath = CopyPictureToPostersFolder(pictureFileName);
                    }               
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
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
        private Movie CreateNewMovieWithRating(string targetPath)
        {
            if (targetPath != string.Empty)
            {
                Movie newMovieWithPicture = new Movie{Name = NewMovieName, Genre = (GenreType)NewMovieGenre, Description = NewMovieDescription,
                    Year = Convert.ToInt32(NewMovieYear), PicturePath = targetPath};
                UserMovie newRatingPictureMovie = new UserMovie{Movie = newMovieWithPicture, UserId = LoggedUser.Id, 
                    Rating = NewMovieRating,Review = NewMovieReview};
                newMovieWithPicture.UserMovies.Add(newRatingPictureMovie);
                return newMovieWithPicture;
            }
            Movie newMovie = new Movie{Name = NewMovieName, Genre = (GenreType)NewMovieGenre, Description = NewMovieDescription, 
                Year = Convert.ToInt32(NewMovieYear)};
            UserMovie newRating = new UserMovie{Movie = newMovie, UserId = LoggedUser.Id, Rating = NewMovieRating, Review = NewMovieReview};
            newMovie.UserMovies.Add(newRating);
            return newMovie;
        }
        private void AddMovieToCollection(Movie newMovie)
        {
            if (newMovie.Name != NewMovieName)
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
            NewMovieGenre = GenreType.Akční;
            NewMovieDescription = string.Empty;
            NewMovieYear = string.Empty;
            NewMoviePicturePath = "Cesta k obrázku";
            NewMovieReview = string.Empty;
            NewMovieRating = 0;
        }       
        private void ResetForm(object? obj)
        {
            _errors.Clear();
            OnErrorsChanged(nameof(NewMovieName));
            OnErrorsChanged(nameof(NewMovieDescription));
            OnErrorsChanged(nameof(NewMovieYear));
            ResetProperties();
        }
    }
}
