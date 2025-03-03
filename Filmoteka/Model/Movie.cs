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
        public int AvgRating 
        {
            get
            {
                if (UserMovies != null)
                {
                    return Convert.ToInt32(UserMovies.Average(x => x.Rating) * 20);

                }
                else
                {
                    return 0;
                }
            }
        } 
        public List<UserMovie>? UserMovies { get; set; }
    }
}
