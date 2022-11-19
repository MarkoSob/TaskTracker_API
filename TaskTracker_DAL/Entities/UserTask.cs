using System.ComponentModel.DataAnnotations;

namespace TaskTracker_DAL.Entities
{
    public class UserTask : Entity
    {
        [StringLength(20, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        public User User { get; set; }
        public UserTask()
        {
            EndDate = DateTime.Now;
        }
    }
}