using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.RolesHelper;
using TaskTracker_DAL.BasicGenericRepository;
using TaskTracker_BL.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.Exceptions.DataAccessExceptions;

namespace TaskTracker_BL.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IBasicGenericRepository<UserRoles> _userRolesRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IRolesHelper _rolesHelper;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            IBasicGenericRepository<UserRoles> userRolesRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<User> userRepository,
            IRolesHelper rolesHelper,
            IMapper mapper,
            ILogger<AdminService> logger
            )
        {
            _userRolesRepository = userRolesRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _rolesHelper = rolesHelper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> GiveRoleAsync(Guid id, string role)
        {
            try
            {
                var roleId = _rolesHelper[role];

                await _userRolesRepository.CreateAsync(new UserRoles
                {
                    UserId = id,
                    RoleId = roleId
                });
            }
            catch
            {
                _logger.LogMessageAndThrowException($"The role {role} doesn't exist", new ObjectNotFoundByKeyException(typeof(Role), role));
            }

            _logger.LogInformation($"The user {id} has been granted the {role} role");

            return true;
        }

        public async Task<bool> RemoveRoleAsync(Guid id, string role)
        {
            try
            {
                var roleId = _rolesHelper[role];

                UserRoles? userRole = _userRolesRepository.GetByPredicate(x => x.UserId == id && x.RoleId == roleId).FirstOrDefault();

                if (userRole != null)
                {
                    _logger.LogInformation($"The user {id} has lost the {role} role");

                    return await _userRolesRepository.DeleteAsync(userRole);
                }
            }
            catch
            {
                _logger.LogMessageAndThrowException($"The role {role} doesn't exist", new ObjectNotFoundByKeyException(typeof(Role), role));
            }

            return false;
        }

        public async Task<bool> SetUserBlockedStatusAsync(Guid id, bool isBLocked)
        {
            User currentUser = _userRepository.GetByPredicate(x => x.Id == id).FirstOrDefault();

            if (currentUser is null)
            {
                _logger.LogMessageAndThrowException($"User {id} does not exist", new ObjectNotFoundException(typeof(User)));
            }

            currentUser.IsBlocked = isBLocked;

            await _userRepository.UpdateAsync(currentUser);

            if (isBLocked)
            {
                _logger.LogInformation($"The user {currentUser.Id} is blocked");
            }
            else
            {
                _logger.LogInformation($"The user {currentUser.Id} is unblocked");
            }

            return true;
        }

        public async Task<IEnumerable<UserForAdminViewDto>> GetAllUsers() =>
             _mapper.Map<IEnumerable<UserForAdminViewDto>>(await _userRepository.GetAllAsync());
    }
}
