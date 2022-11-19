using System.ComponentModel.DataAnnotations;

namespace TaskTracker_BL.DTOs
{
    public class CredentialsDto
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
