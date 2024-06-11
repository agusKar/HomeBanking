using HomeBanking.DTOs;
using System.Security.Claims;

namespace HomeBanking.Services
{
    public interface IAuthService
    {
        ClaimsIdentity GenerateClaim(LoginDTO LoginDTO);
        string GenerateJWT(LoginDTO LoginDTO);
    }
}
