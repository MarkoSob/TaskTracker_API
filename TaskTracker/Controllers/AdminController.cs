using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.AdminService;
using TaskTracker_DAL.Entities;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = RolesList.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("GiveRole")]
        public async Task<IActionResult> GiveRoleAsync(Guid id, string role)
        {
            await _adminService.GiveRoleAsync(id, role);
            return Ok();
        }

        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRoleAsync(Guid id, string role) =>
            Ok(await _adminService.RemoveRoleAsync(id, role));
    
        [HttpPut("BlokedStatus")]
        public async Task<IActionResult> SetUserBlockedStatusAsync(Guid id, bool isBlocked) =>
            Ok(await _adminService.SetUserBlockedStatusAsync(id, isBlocked));

        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers() =>
            Ok(await _adminService.GetAllUsers());
    }
}
