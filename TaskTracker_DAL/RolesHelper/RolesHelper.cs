using TaskTracker_DAL.Entities;

namespace TaskTracker_DAL.RolesHelper
{
    public class RolesHelper : IRolesHelper
    {
        private Dictionary<string, Guid> _roles;

        public RolesHelper()
        {
            _roles = new Dictionary<string, Guid>()
            {
                { RolesList.Admin, Guid.Parse("4e506bed-0876-4e8b-a4ca-15d6167c5c97") },
                { RolesList.User, Guid.Parse("a2a9a6ba-cc43-4251-bfc9-34791264a417") }
            };
        }

        public Guid this[string roleTitle]
            => _roles[roleTitle];
    }
}
