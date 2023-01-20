using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Exceptions.DataAccessExceptions;
using TaskTracker.Core.Exceptions.DomainExceptions;
using TaskTracker.Core.Extensions;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;

namespace TaskTracker_BL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IGenericRepository<User> userRepository,
                IMapper mapper,
                ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserProfileDataDto> GetUserDataAsync(string email)
        {
            User user = await _userRepository.GetByPredicate(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundException(typeof(User)));
            }

            var userData = _mapper.Map<UserProfileDataDto>(user);

            return userData;

        }

        public async Task<UserProfileDataDto> UpdateUserDataAsync(Guid? id, UserDataForUpdateDto user)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
            }

            if (user is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundException(typeof(UserProfileDataDto)));
            }

            User userForUpdate = await _userRepository.GetByPredicate(u => u.Id == id).FirstOrDefaultAsync();

            userForUpdate.FirstName = user.FirstName;
            userForUpdate.LastName = user.LastName;

            var updatedUser = _mapper.Map<UserProfileDataDto>(await _userRepository.UpdateAsync(userForUpdate));

            _logger.LogInformation($"The user {updatedUser.Email} with id {updatedUser.Id} was updated");

            return updatedUser;
        }
    }
}
