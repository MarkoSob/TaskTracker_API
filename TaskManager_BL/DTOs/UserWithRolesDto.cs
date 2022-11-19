using TaskTracker_DAL.Entities;

namespace TaskTracker_BL.DTOs
{
    public class UserWithRolesDto
    {
        public User User { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
