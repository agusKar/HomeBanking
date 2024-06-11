using HomeBanking.DTOs;
using HomeBanking.Utilities;
using HomeBanking.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HomeBanking.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        private readonly IConfiguration _config;

        public AuthService(IClientService clientService, IConfiguration config)
        {
            _clientService = clientService;
            _config = config;
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
        public string GenerateJWT(LoginDTO LoginDTO)
        {
            try
            {
                Client user = _clientService.GetClientByEmail(LoginDTO.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(LoginDTO.Password, user.Password))
                {
                    throw new CustomException("No se encontro al usuario", 401);
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                
                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };
                if (user.Email == "agustin@gmail.com")
                {
                    claims.Add(new Claim("Admin", "true"));
                }

                var Sectoken = new JwtSecurityToken(
                  null,
                  null,
                  claims,
                  expires: DateTime.Now.AddMinutes(1),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message, 401);
            }
        }
    }
}
