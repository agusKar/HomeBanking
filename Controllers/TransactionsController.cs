using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        public readonly IClientService _clientService;
        public readonly IAccountService _accountService;
        public readonly ITransactionService _transactionService;

        //crear constructor con todos los repositorios que voy a usar
        public TransactionsController(
            IClientService clientService,
            IAccountService accountService,
            ITransactionService transactionService
         )
        {
            _clientService = clientService;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        /*
         * crear endpoint POST que reciba el DTO con 4 parametros 
         */
        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateTransactions(TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientService.GetClientByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }

                TransferReturnDTO transferReturnDTO = _transactionService.CreateTransaction(transferDTO, client.Id);

                return Ok(transferReturnDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
