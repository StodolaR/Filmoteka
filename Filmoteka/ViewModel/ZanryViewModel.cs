﻿using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka.ViewModel
{
    public class ZanryViewModel 
    {
        ZebricekViewModel zebricekViewModel = new ZebricekViewModel();
        public ObservableCollection<Movie> Movies { get; set; } 
        public ZanryViewModel()
        {
            Database database = new Database();
            Movies = database.Movies;

        }
    }
}
