using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.RolesHelper;
using TaskTracker_DAL.BasicGenericRepository;

namespace TaskTracker_BL.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IBasicGenericRepository<UserRoles> _userRolesRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IRolesHelper _rolesHelper;
        private readonly IGenericRepository<User> _userRepository;

        public AdminService(
            IBasicGenericRepository<UserRoles> userRolesRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<User> userRepository,
            IRolesHelper rolesHelper
            )
        {
            _userRolesRepository = userRolesRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _rolesHelper = rolesHelper;
        }

        public async Task GiveRoleAsync(Guid id, string role)
        {
            var roleId = _rolesHelper[role];

            await _userRolesRepository.CreateAsync(new UserRoles
            {
                UserId = id,
                RoleId = roleId
            });
        }
        public async Task<bool> RemoveRoleAsync(Guid id, string role)
        {
            var roleId = _rolesHelper[role];

            UserRoles? userRole = _userRolesRepository.GetByPredicate(x => x.UserId == id && x.RoleId == roleId).FirstOrDefault();

            if (userRole != null)
            {
                return await _userRolesRepository.DeleteAsync(userRole);
            }

            return false;
        }

        public async Task<bool> SetUserBlockedStatusAsync(string email, bool isBLocked)
        {
            User currentUser = _userRepository.GetByPredicate(x => x.Email == email).FirstOrDefault();
            if(currentUser != null)
            {
                currentUser.IsBlocked = isBLocked;
                await _userRepository.UpdateAsync(currentUser);
                return true;
            }
            return false;
        }

    }
}
