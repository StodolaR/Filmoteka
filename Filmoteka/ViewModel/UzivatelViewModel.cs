using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class UzivatelViewModel 
    {
        public ObservableCollection<User> Users { get; set; }
        public UzivatelViewModel()
        {
            Database database = new Database();
            Users = database.Users;


        }
    }
}
