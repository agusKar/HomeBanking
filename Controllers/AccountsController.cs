using HomeBanking.DTOs;
using HomeBanking.Utilities;
using HomeBanking.Repositories;
using HomeBanking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public ActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountService.GetAllAccounts();
                var accountsDTO = accounts.Select(accounts => new AccountDTO(accounts)).ToList();
                return Ok(accountsDTO);

            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "ClientOnly")]
        public ActionResult GetAccount(long id) {
            try
            {
                var account = _accountService.GetAccountById(id);
                var accountDTO = new AccountDTO(account);
                return Ok(accountDTO);
            } catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
    }
}
