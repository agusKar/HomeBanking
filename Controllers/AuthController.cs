using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using HomeBanking.DTOs;
using HomeBanking.Services;
using System.Security.Claims;
using HomeBanking.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HomeBanking.Models;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClientService _clientService;

        public AuthController(IAuthService authService, IClientService clientService)
        {
            _clientService = clientService;
            _authService = authService;
        }

        [HttpPost("loginJWT")]
        public IActionResult LoginJWT([FromBody] LoginDTO LoginDTO)
        {
            try
            {
                //Client user = _clientService.GetClientByEmail(LoginDTO.Email);

                //if (user == null || !BCrypt.Net.BCrypt.Verify(LoginDTO.Password, user.Password))
                //{
                //    throw new CustomException("No se encontro al usuario", 401);
                //}

                string tokenReceived = _authService.GenerateJWT(LoginDTO);

                return Ok(tokenReceived);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        [HttpPost("login")]
        //Retornará una tarea (Task) de tipo IActionResult, la tarea es una manera que tiene el framework de .Net para representar trabajos que se pueden ejecutar de manera asíncrona
        public async Task<IActionResult> Login([FromBody] LoginDTO LoginDTO)
        {
            try
            {
                ClaimsIdentity claimsIdentity = _authService.GenerateClaim(LoginDTO);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return Ok();
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
    }
}