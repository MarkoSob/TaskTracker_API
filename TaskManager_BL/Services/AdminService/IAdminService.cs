
namespace TaskTracker_BL.Services.AdminService
{
    public interface IAdminService
    {
        Task GiveRoleAsync(Guid id, string role);
        Task<bool> RemoveRoleAsync(Guid id, string role);
        Task<bool> BlockUserAsync(string email);
        Task<bool> UnblockUserAsync(string email);
    }
}