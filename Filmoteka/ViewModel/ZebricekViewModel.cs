using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZebricekViewModel : AbstractTabViewModel
    {
        public ObservableCollection<Film> Films { get; set; } 
        
        public ZebricekViewModel()
        {
            Header = "Žebříček";
            Films = new ObservableCollection<Film>()
            {
                new Film() { Name = "Rambo", Description = "Válečný veterán po návratu domů bojuje s policií", Genre = GenreType.Akční, Year = 1983},
                new Film() { Name = "Thing", Description = "Vědce za polárním kruhem likviduje neznámý organismus", Genre = GenreType.Horor, Year = 1976},
                new Film() { Name = "Žhavé výstřely", Description = "Parodie na akční filmy", Genre = GenreType.Komedie, Year = 1993}
            };
        }
    }
}
