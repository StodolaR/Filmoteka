using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZanryViewModel : AbstractTabViewModel
    {
        ZebricekViewModel zebricekViewModel = new ZebricekViewModel();
        public ObservableCollection<Film> Films { get; set; } 
        public ZanryViewModel()
        {
            Header = "Žánry";

        }
    }
}
