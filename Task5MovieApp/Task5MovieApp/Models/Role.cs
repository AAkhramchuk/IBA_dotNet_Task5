using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task5MovieApp.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string UserRole { get; set; }
    }
}
