using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filmoteka.ViewModel
{
    public class MainWindowViewModel 
    {
        public Database Database { get; set; }
        public UvodViewModel UvodViewModel { get; set; }
        public ZebricekViewModel ZebricekViewModel { get; set; }
        public ZanryViewModel ZanryViewModel { get; set; }
        public UzivatelViewModel UzivatelViewModel { get; set; }
        public PrihlaseniViewModel PrihlaseniViewModel { get; set; }
        public ObservableCollection<AbstractTabViewModel> Items { get; set; }
        public User? LoggedUser { get; set; }
        public MainWindowViewModel()
        {
            Database = new Database();
            UvodViewModel = new UvodViewModel();
            ZebricekViewModel = new ZebricekViewModel();
            ZanryViewModel = new ZanryViewModel();
            UzivatelViewModel = new UzivatelViewModel();
            PrihlaseniViewModel = new PrihlaseniViewModel();

            ZebricekViewModel.Films = Database.Films;
            //for (int i = 0; i < database.Films.Count; i++)
            //{
            //    zebricekViewModel.Films.Add(new Film { Name = database.Films[i].Name, Year = database.Films[i].Year, 
            //        PicturePath = database.Films[i].PicturePath, Genre= database.Films[i].Genre });
            //}
            
            Items = new ObservableCollection<AbstractTabViewModel>();
            Items.Add(UvodViewModel);
            Items.Add(ZebricekViewModel);
            Items.Add(ZanryViewModel);
            Items.Add(UzivatelViewModel);
            Items.Add(PrihlaseniViewModel);
        }

        
        
    }
}
