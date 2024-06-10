using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories;
using HomeBanking.Utilities;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace HomeBanking.Services.Implementations
{
    public class ClientLoanService : IClientLoanService
    {
        private readonly ILoanService _loanService;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IClientLoanRepository _clientLoanRepository;


        public ClientLoanService(
            ILoanService loanService,
            IAccountService accountService,
            ITransactionService transactionService,
            IClientLoanRepository clientLoanRepository
        )
        {
            _loanService = loanService;
            _accountService = accountService;
            _transactionService = transactionService;
            _clientLoanRepository = clientLoanRepository;
        }

        public void AsignLoanToClient(LoanApplicationDTO loanApplicationDTO, Client Client)
        {
            try
            {
                // Verificar que el prestamo seleccionado exista
                var loanFinded = _loanService.GetLoanById(loanApplicationDTO.loanId) ?? throw new CustomException("El prestamo no existe", 403);

                // Que el monto NO sea 0 y que no sobrepase el máximo autorizado.
                if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount > loanFinded.MaxAmount)
                {
                    throw new CustomException("El monto es 0 o supera el máximo permitido", 403);
                }

                // Que los payments no lleguen vacíos.,
                List<int> listPayments = loanFinded.Payments.Split(",").Select(int.Parse).ToList();

                if (loanApplicationDTO.Payments.IsNullOrEmpty() || !listPayments.Contains(int.Parse(loanApplicationDTO.Payments)))
                {
                    throw new CustomException("El payment fue mal asignado", 403);
                }

                // Que exista la cuenta de destino
                var accountFinded = _accountService.GetAccountByNumber(loanApplicationDTO.toAccountNumber) ?? throw new Exception("La cuenta seleccionada no existe.");

                // Que la cuenta de destino pertenezca al Cliente autentificado
                if (accountFinded.ClientId != Client.Id)
                {
                    throw new CustomException("La cuenta seleccionada no pertenece al mismo cliente", 403);
                }

                // Cuando guardes clientLoan el monto debes multiplicarlo por el 20 %.
                var newAmount = loanApplicationDTO.Amount * 1.20;
                ClientLoan clientLoan = new ClientLoan()
                {
                    Amount = newAmount,
                    ClientId = Client.Id,
                    LoanId = loanApplicationDTO.loanId,
                    Payments = loanApplicationDTO.Payments
                };
                _clientLoanRepository.SaveClientLoan(clientLoan);

                // Guardar la transaccción
                Transaction newTransaction = new Transaction()
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = loanApplicationDTO.Amount,
                    Description = loanFinded.Name + "- Loan approved",
                    Date = DateTime.Now,
                    AccountId = accountFinded.Id
                };
                int transSave = _transactionService.SaveTransaction(newTransaction);

                // Actualizar el Balance de la cuenta sumando el monto del préstamo
                accountFinded.Balance += loanApplicationDTO.Amount;
                // Guardar la cuenta
                _accountService.UpdateAccount(accountFinded);
            }
            catch (CustomException e)
			{
                throw new CustomException(e.Message, 403);
			}
        }
    }
}
