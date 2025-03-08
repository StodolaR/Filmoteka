using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Filmoteka.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GenreType Genre { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public string PicturePath { get; set; } = "/Resources/bezobrazku.png";
        public List<UserMovie>? UserMovies { get; set; } = new List<UserMovie>();
    }
}
