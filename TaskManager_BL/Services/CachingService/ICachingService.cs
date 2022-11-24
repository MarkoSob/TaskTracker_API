
namespace TaskTracker_BL.Services.CachingService
{
    public interface ICachingService
    {
        Task<string> GetAsync(string key);
        Task SaveAsync(string key, string value);
    }
}