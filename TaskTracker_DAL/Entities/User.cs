using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker_DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : Entity
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRoles>? Roles { get; set; }
        public ICollection<UserTask>? Tasks { get; set; }
    }
}
