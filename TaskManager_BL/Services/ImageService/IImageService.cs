
using Microsoft.AspNetCore.Http;

namespace TaskTracker_BL.Services.ImageService
{
    public interface IImageService
    {
        Task<bool> AddImageAsync(IFormFile uploadedFile, string email);
    }
}