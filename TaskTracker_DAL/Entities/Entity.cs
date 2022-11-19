using System.ComponentModel.DataAnnotations;

namespace TaskTracker_DAL.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
