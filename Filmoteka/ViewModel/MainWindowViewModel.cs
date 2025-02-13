using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class MainWindowViewModel
    {
        public ObservableCollection<AbstractTabViewModel> Items { get; set; }
        public MainWindowViewModel()
        {
            Items = new ObservableCollection<AbstractTabViewModel>();
            Items.Add(new UvodViewModel());
            Items.Add(new ZebricekViewModel());
            Items.Add(new ZanryViewModel());
            Items.Add(new UzivatelViewModel());
            Items.Add(new PrihlaseniViewModel());
        }
    }
}
