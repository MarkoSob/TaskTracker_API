using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.UserService
{
    public interface IUserService
    {
        Task<UserProfileDataDto> GetUserDataAsync(string email);
    }
}