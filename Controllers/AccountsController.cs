using HomeBanking.DTOs;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet]
        public ActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
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
                var account = _accountRepository.GetAccountById(id);
                var accountDTO = new AccountDTO(account);
                return Ok(accountDTO);
            } catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
