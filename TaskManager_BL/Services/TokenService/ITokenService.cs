using System.Security.Claims;

namespace TaskTracker_BL.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(string userName, IEnumerable<string> userRoles);
        ClaimsPrincipal GetPrincipal(string token);
        string ValidateToken(string token);
    }
}
