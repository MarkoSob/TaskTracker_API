using System.ComponentModel.DataAnnotations;
using TaskTracker_DAL.Entities;

namespace TaskTracker_BL.DTOs
{
    public class CreateUserTaskDto
    {
        [StringLength(20, MinimumLength = 1)]
        [Required]
        public string? Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public string? Status { get; set; }
        public string? UserEmail { get; set; }
    }
}
