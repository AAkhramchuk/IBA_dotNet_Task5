using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task5MovieApp.Models
{
    [Table("UserData")]
    public class UserData
    {
        [Key]
        public int Id { get; set; }
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
        public int GenreID { get; set; }
        public Genre Genre { get; set; }
        public int UserID { get; set; }
        public UserProfile UserProfile { get; set; }
        [MaxLength(50)]
        public string? UserComment { get; set; }
        public int? MovieRating { get; set; }
    }
}
