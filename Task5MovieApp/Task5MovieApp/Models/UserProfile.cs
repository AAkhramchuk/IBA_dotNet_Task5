using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task5MovieApp.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        // user ID from AspNetUser table.
        public string? OwnerID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
