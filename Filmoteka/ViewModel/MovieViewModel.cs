using Filmoteka.Framework;
using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    class MovieViewModel :ViewModelBase
    {
        private int avgRating;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GenreType Genre { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public string PicturePath { get; set; } = "/Resources/bezobrazku.png";
        public ObservableCollection<UserMovie> Ratings { get; set; } = new ObservableCollection<UserMovie>();
        public int AvgRating 
        {
            get => avgRating;
            set 
            {
                avgRating = value;
                OnPropertyChanged(nameof(AvgRating));
            }
        }
    }
}
