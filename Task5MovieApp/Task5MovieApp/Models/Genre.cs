using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task5MovieApp.Models
{
    [Table("Genre")]
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Movie Genre"), StringLength(50, MinimumLength = 3)]
        public string MovieGenre { get; set; }
    }
}
