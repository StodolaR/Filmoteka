using Filmoteka.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmoteka
{
    public class Database
    {
        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<UserMovie> Ratings { get; set; }

        public Database()
        {
            Movies = new ObservableCollection<Movie>()
            {
                new Movie() { Name = "Rambo", Description = "Válečný veterán po návratu domů bojuje s policií", Genre = GenreType.Akční,
                    Year = 1983, PicturePath = "/Resources/Posters/Rambo.jpg"},
                new Movie() { Name = "Thing", Description = "Vědce za polárním kruhem likviduje neznámý organismus", Genre = GenreType.Horor,
                    Year = 1976, PicturePath = "/Resources/Posters/Thing.jpg"},
                new Movie() { Name = "Žhavé výstřely", Description = "Parodie na Top Gun", Genre = GenreType.Komedie,
                    Year = 1991, PicturePath = "/Resources/Posters/ZhaveVystrely.jpg"}
            };
            Users = new ObservableCollection<User>()
            {
            
                new User { Name = "Admin", Password = "AdminABC"},
                new User { Name = "Uzivatel1", Password = "Uzivatel1ABC"}

            };
            Ratings = new ObservableCollection<UserMovie>()
            {
                
            };
        }
    }
}
