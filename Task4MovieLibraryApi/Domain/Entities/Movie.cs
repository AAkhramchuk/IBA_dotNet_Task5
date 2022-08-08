using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Movie data model
    /// </summary>
    [Table("Movie")]
    public class Movie
    {
        public int ID { get; set; }

        [Display(Name = "Producer Name"), StringLength(50, MinimumLength = 3)]
        public string? ProducerName { get; set; }

        [Display(Name = "Producer Surname"), StringLength(50, MinimumLength = 3)]
        public string? ProducerSurname { get; set; }

        [Display(Name = "Movie Name"), StringLength(50, MinimumLength = 3)]
        public string? MovieName { get; set; }

        [Display(Name = "Release Year"), Range(1900, 2100), StringLength(4)]
        public int? MovieYear { get; set; }

        [Display(Name = "Rating"), Range(1, 100), StringLength(3)]
        public int? MovieRating { get; set; }
    }
}