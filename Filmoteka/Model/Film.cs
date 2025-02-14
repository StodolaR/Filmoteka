using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Filmoteka.Model
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GenreType Genre { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string PicturePath { get; set; } = "/Resources/bezobrazku.png";
        public int AvgRating { get; set; }
        
    }
}
