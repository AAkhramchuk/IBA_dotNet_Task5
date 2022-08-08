using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Task5MovieApp.Models
{
    public class UserDataViewModel
    {
        public int UserDataID { get; set; }
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
        public List<Genre> GenreList { get; set; }
        public UserData UserData { get; set; }
        public List<UserData> UserDataList { get; set; }
    }
}
