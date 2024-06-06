using HomeBanking.DTOs;
using HomeBanking.Repositories;
using HomeBanking.Services;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountService.GetAllAccounts();
                var accountsDTO = accounts.Select(accounts => new AccountDTO(accounts)).ToList();
                return Ok(accountsDTO);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult GetAccount(long id) {
            try
            {
                var account = _accountService.GetAccountById(id);
                var accountDTO = new AccountDTO(account);
                return Ok(accountDTO);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
