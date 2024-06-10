using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using HomeBanking.DTOs;
using HomeBanking.Services;
using System.Security.Claims;
using HomeBanking.Utilities;
using System.Net;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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