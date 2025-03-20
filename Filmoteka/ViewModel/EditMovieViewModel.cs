using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            } 
        }
        public string? EditedYear
        {
            get => editedYear;
            set
            {
                editedYear = value;
                OnPropertyChanged(nameof(EditedYear));
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
    }
}
