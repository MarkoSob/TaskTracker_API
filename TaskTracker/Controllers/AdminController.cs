using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.AdminService;
using TaskTracker_DAL.Entities;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = RolesList.Admin)]
        [HttpPost("GiveRole")]
        public async Task<IActionResult> GiveRoleAsync(Guid id, string role)
        {
            await _adminService.GiveRoleAsync(id, role);
            return Ok();
        }

        [Authorize(Roles = RolesList.Admin)]
        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRoleAsync(Guid id, string role) =>
            Ok(await _adminService.RemoveRoleAsync(id, role));
    

        //[Authorize(Roles = RolesList.Admin)]
        [HttpPut("BlokedStatus")]
        public async Task<IActionResult> SetUserBlockedStatusAsync(string email, bool isBlocked) =>
            Ok(await _adminService.SetUserBlockedStatusAsync(email, isBlocked));

       

    }
}
