using System.ComponentModel.DataAnnotations;

namespace TaskTracker_BL.DTOs
{
    public class UserDataForUpdateDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
