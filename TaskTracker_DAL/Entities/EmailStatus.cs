using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker_DAL.Entities
{
    public class EmailStatus : Entity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? Key { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
