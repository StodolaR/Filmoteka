using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    class MainWindowViewModel
    {
        public ObservableCollection<AbstractTabViewModel> Items { get; set; }
        public MainWindowViewModel()
        {
            Items = new ObservableCollection<AbstractTabViewModel>();
            Items.Add(new UvodViewModel("Úvod"));
            Items.Add(new ZebricekViewModel("Žebříček"));
            Items.Add(new ZanryViewModel("Žánry"));
            Items.Add(new UzivatelViewModel("Uživatelé"));
            Items.Add(new PrihlaseniViewModel("Přihlášení"));
        }
    }
}
