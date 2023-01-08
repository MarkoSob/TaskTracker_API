using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Services;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private const string ConfirmationRoute = "confrimation";

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationDto registrationDto)
        {
            var contoller = Request.RouteValues["controller"]!.ToString();
            UriBuilder uriBuilder = new UriBuilder()
            {
                Scheme = Request.Scheme,
                Host = Request.Host.Host,
                Path = contoller + "/" + ConfirmationRoute
            };
            await _authService.RegisterAsync(registrationDto, uriBuilder);
            return Ok();
        }

        [Authorize]
        [HttpPut("changepassword/{email}")]
        public async Task<IActionResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            await _authService.ChangePasswordAsync(email, currentPassword, newPassword);
            return Ok();
        }

        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync(string email)
        {
            await _authService.ResetPasswordAsync(email);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(CredentialsDto credentialsDto) =>
             Ok(await _authService.LoginAsync(credentialsDto));

        [HttpGet(ConfirmationRoute)]
        public async Task<IActionResult> ConfirmEmailAsync(string email, string key) => 
            Ok(await _authService.ConfirmEmailAsync(email, key));
    }
}
