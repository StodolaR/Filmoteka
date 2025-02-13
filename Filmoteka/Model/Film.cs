using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.Model
{
    internal class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GenreType Genre { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public Uri Picture { get; set; }
        public int AvgRating { get; set; }
        
    }
}
