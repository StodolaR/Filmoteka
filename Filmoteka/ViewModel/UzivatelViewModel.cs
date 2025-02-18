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
            Users = new ObservableCollection<User>();
            using(MovieContext mc = new MovieContext())
            {
                foreach (User user in mc.Users)
                {
                    Users.Add(user);
                }
            }


        }
    }
}
