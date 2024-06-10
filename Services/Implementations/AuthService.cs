using HomeBanking.DTOs;
using HomeBanking.Utilities;
using HomeBanking.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Security.Claims;

namespace HomeBanking.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public ClaimsIdentity GenerateClaim(LoginDTO LoginDTO)
        {
            try
            {
                Client user = _clientService.GetClientByEmail(LoginDTO.Email);

                //if (user == null || !String.Equals(user.Password, LoginDTO.Password))
                if(user == null || !BCrypt.Net.BCrypt.Verify(LoginDTO.Password,user.Password))
                {
                    throw new CustomException("No se encontro al usuario", 401);
                }
                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };

                if (user.Email == "agustin@gmail.com")
                {
                    claims.Add(new Claim("Admin", "true"));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                return claimsIdentity;
            }
			catch (Exception e)
			{
                throw new CustomException(e.Message, 401);
			}
        }
    }
}
