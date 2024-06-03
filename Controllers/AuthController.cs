﻿using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public AuthController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost("login")]
        //Retornará una tarea (Task) de tipo IActionResult, la tarea es una manera que tiene el framework de .Net para representar trabajos que se pueden ejecutar de manera asíncrona
        public async Task<IActionResult> Login([FromBody] Client client)
        {
            try
            {
                Client user = _clientRepository.FindByEmail(client.Email);
                if (user == null || !String.Equals(user.Password, client.Password)) {
                    return Unauthorized();
                }
                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}