using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.AdminService
{
    public interface IAdminService
    {
        Task<bool> GiveRoleAsync(Guid id, string role);
        Task<bool> RemoveRoleAsync(Guid id, string role);
        Task<bool> SetUserBlockedStatusAsync(string email, bool isBLocked);
        Task<IEnumerable<UserForAdminViewDto>> GetAllUsers();
    }
}