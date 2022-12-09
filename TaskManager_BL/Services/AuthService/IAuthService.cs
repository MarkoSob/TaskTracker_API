using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services
{
    public interface IAuthService
    {
        Task<bool> ConfirmEmailAsync(string email, string key);
        Task<string> LoginAsync(CredentialsDto credentialsDto);
        Task RegisterAsync(RegistrationDto registartionDto, UriBuilder uriBuilder);
        Task<bool> ChangePasswordAsync(string email, string currentPasswoord, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
    }
}