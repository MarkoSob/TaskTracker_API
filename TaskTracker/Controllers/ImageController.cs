using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
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

        [HttpPost("upload")]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, string email)
        {
            var result = await _imageService.AddImageAsync(uploadedFile, email);

            return Ok(result);
        }
    }
}
