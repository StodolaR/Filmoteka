using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZebricekViewModel 
    {
        public ObservableCollection<Film> Films { get; set; } 
        
        public ZebricekViewModel()
        {
            Database database = new Database();
            Films = database.Films;
            
        }
        
    }
}
