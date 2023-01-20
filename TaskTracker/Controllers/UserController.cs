using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Services.UserService;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDataAsync(string email)
        {
            var result = await _userService.GetUserDataAsync(email);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUSerDataAsync(Guid id, UserDataForUpdateDto user)
        {
            var result = await _userService.UpdateUserDataAsync(id, user);

            return Ok(result);
        }
    }
}
