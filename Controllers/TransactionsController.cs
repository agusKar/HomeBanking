using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories;
using HomeBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

                //Verificar que los parámetros no estén vacíos
                if (transferDTO.Amount == 0 || transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty() || transferDTO.Description == "")
                {
                    return StatusCode(403, "Los datos no fueron ingresados correctamente.");
                }

                //Verificar que los números de cuenta no sean iguales
                if (transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber == transferDTO.FromAccountNumber)
                {
                    return StatusCode(403, "Los datos de cuenta son iguales. Operacion Invalida.");
                }

                //Verificar que exista la cuenta de origen
                var fromAccount = _accountService.GetAccountByNumber(transferDTO.FromAccountNumber);
                if (fromAccount == null)
                {
                    return StatusCode(403, "La cuenta de origen no existe.");
                }

                //Verificar que la cuenta de origen pertenezca al cliente autenticado
                if(fromAccount.ClientId != client.Id)
                {
                    return StatusCode(403, "La cuenta de origen no pertenece al cliente autenticado.");
                }

                //Verificar que exista la cuenta de destino
                var toAccount = _accountService.GetAccountByNumber(transferDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return StatusCode(403, "La cuenta de destino no existe.");
                }

                // Verificar que la cuenta de origen tenga el monto disponible.
                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return StatusCode(403, "La cuenta no tiene monto disponible para realizar esta operacion.");
                }

                /*
                 * Se deben crear dos transacciones,
                 * -- Una con el tipo de transacción “DEBIT” asociada a la cuenta de origen
                 * -- La otra con el tipo de transacción “CREDIT” asociada a la cuenta de destino.
                */

                var fromTransacction = new Transaction
                {
                    Type = TransactionType.DEBIT.ToString(),
                    Amount = -transferDTO.Amount,
                    Description = transferDTO.FromAccountNumber +"-"+transferDTO.Description,
                    Date = DateTime.Now,
                    AccountId = fromAccount.Id
                };
                var toTransacction = new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = transferDTO.Amount,
                    Description = transferDTO.ToAccountNumber + "-" + transferDTO.Description,
                    Date = DateTime.Now,
                    AccountId = toAccount.Id

                };

                _transactionService.SaveTransaction(fromTransacction);
                _transactionService.SaveTransaction(toTransacction);

                var fromTransactionInDB = _transactionService.GetTransactionsById(fromTransacction.Id);
                var toTransactionInDB = _transactionService.GetTransactionsById(toTransacction.Id);
                if (fromTransactionInDB != null && toTransactionInDB != null)
                {
                    /*
                     * A la cuenta de origen se le restará el monto indicado en la petición
                     * a la cuenta de destino se le sumará el mismo monto.
                    */
                    var fromAccountDB = _accountService.GetAccountById(fromAccount.Id);
                    var toAccountDB = _accountService.GetAccountById(toAccount.Id);

                    if(fromAccountDB != null && toAccountDB != null)
                    {
                        fromAccountDB.Balance += fromTransacction.Amount;
                        toAccountDB.Balance += toTransacction.Amount;

                        _accountService.SaveAccount(fromAccountDB);
                        _accountService.SaveAccount(toAccountDB);
                    }
                    else
                    {
                        return StatusCode(500, "Error en la actualizacion de cuentas.");
                    }
                }
                else
                {
                    return StatusCode(500, "Error en la carga de transacciones.");
                }


                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
