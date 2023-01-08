using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.CachingService;
using TaskTracker_DAL.Entities;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionController : ControllerBase
    {
        private ICachingService _cachingService;

        public ActionController(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        [HttpPost]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            await _cachingService.SaveAsync(key, value);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetValue(string key)
        {
            await _cachingService.GetAsync(key);
            return Ok();
        }

        [Authorize(Roles = RolesList.User)]
        [HttpGet(nameof(User))]
        public IActionResult User()
        {
            return Ok();
        }

        [Authorize(Roles = RolesList.Admin)]
        [HttpGet(nameof(Admin))]
        public IActionResult Admin()
        {
            return Ok();
        }

        [Authorize(Roles = RolesList.User)]
        [Authorize(Roles = RolesList.Admin)]
        [HttpGet(nameof(UserAdmin))]
        public IActionResult UserAdmin()
        {
            return Ok();
        }

        [Authorize(Roles = RolesList.User + "," + RolesList.Admin)]
        [HttpGet(nameof(UserOrAdmin))]
        public IActionResult UserOrAdmin()
        {
            return Ok();
        }
    }
}
