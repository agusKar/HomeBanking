using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Services;
using HomeBanking.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IClientLoanService _clientLoanService;
        private readonly ILoanService _loanService;
        public LoansController(
            IClientService clientService, 
            ILoanService loanService,
            IClientLoanService clientLoanService
        )
        {
            _clientService = clientService;
            _loanService = loanService;
            _clientLoanService = clientLoanService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Client GetCurrentClient()
        {
            string email = User.FindFirst("Client")?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                throw new CustomException("Usuario no encontrado", HttpStatusCode.Forbidden);
            }

            return _clientService.GetClientByEmail(email);
        }

        // GET /api/loans
        [HttpGet]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetLoan()
        {
            try
            {
                Client clientCurrent = GetCurrentClient();
                /*
                    * traer todos Loan                 
                 */
                return Ok(_loanService.GetAllLoans());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // POST /api/loans
        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateLoan(LoanApplicationDTO LoanApplicationDTO)
        {
            try
            {
                Client clientCurrent = GetCurrentClient();
                _clientLoanService.AsignLoanToClient(LoanApplicationDTO, clientCurrent);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
