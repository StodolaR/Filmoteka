using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.Model
{
    public class UserMovie
    {
        public int MovieId {  get; set; }
        public Movie Movie { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
    }
}
