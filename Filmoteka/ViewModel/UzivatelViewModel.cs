using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class UzivatelViewModel : AbstractTabViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public UzivatelViewModel()
        {
            Header = "Uživatelé";

            Users = new ObservableCollection<User>()
            {
                new User { Name = "Admin", Password = "AdminABC"}
            };
        }
    }
}
