using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    abstract class MovieOperationViewModel : UserMovieViewmodel
    {
        private string? newMovieName = string.Empty;
        private GenreType? newMovieGenre = GenreType.Akční;
        private string? newMovieDescription = string.Empty;
        private string newMovieYear = string.Empty;
        private string? newMoviePicturePath = "Cesta k obrázku";
        private string message = string.Empty;
        public string? NewMovieName
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
        public GenreType? NewMovieGenre
        {
            get => newMovieGenre;
            set
            {
                newMovieGenre = value;
                OnPropertyChanged(nameof(newMovieGenre));
            }
        }
        public string? NewMovieDescription
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
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        protected MovieOperationViewModel(UserCollectionViewModel userCollectionViewModel, MovieCollectionViewModel movieCollectionViewModel) 
            : base(userCollectionViewModel, movieCollectionViewModel)
        {
        }
        protected void CheckErrors(string propertyName)
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
                        AddError(propertyName, "Rok výroby mimo rozsah (1900 - letošní rok)"); break;
            }
        }
        protected void CreateDirectoryIfNotExist()
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
        protected string CheckFileNameUniqueness()
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
        protected string CopyPictureToPostersFolder(string pictureFileName)
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
    }
}
