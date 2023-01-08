using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.ImageService;

namespace FileUploadApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private IWebHostEnvironment _appEnvironment;
        private IImageService _imageService;

        public ImageController(IWebHostEnvironment appEnvironment, IImageService imageService)
        {
            _appEnvironment = appEnvironment;
            _imageService = imageService;
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> AddImageAsync(IFormFile uploadedFile, string email)
        {
            var result = await _imageService.AddImageAsync(uploadedFile, email);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetImagePathAsync(string email)
        {
            var result = await _imageService.GetImagePathAsync(email);

            return Ok(result);
        }
    }
}
