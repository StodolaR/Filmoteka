using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.Model
{
    public class Rating
    {
        public int FilmId {  get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string? Review { get; set; }
    }
}
