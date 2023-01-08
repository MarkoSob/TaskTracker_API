using System.ComponentModel.DataAnnotations;
using TaskTracker_DAL.Entities;

namespace TaskTracker_BL.DTOs
{
    public class CreateUserTaskDto
    {
        [StringLength(20, MinimumLength = 1)]
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? UserEmail { get; set; }
    }
}
