using HomeBanking.Models;
using HomeBanking.Repositories;
using HomeBanking.Repositories.Implementations;
using System.Security.Cryptography;

namespace HomeBanking.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public int GetCountAccountsByClient(long clientId)
        {
            var accountsByClient = _accountRepository.GetAccountsByClient(clientId);
            return accountsByClient.Count();
        }

        public string GetRandomAccountNumber()
        {
            string NumberAccountRandom;
            do
            {
                NumberAccountRandom = "VIN-" + RandomNumberGenerator.GetInt32(0, 99999999);

            } while (_accountRepository.GetAccountByNumber(NumberAccountRandom) != null);
            return NumberAccountRandom;
        }
        public void SaveAccount(Account account)
        {
            try
            {
                _accountRepository.SaveAccount(account);
            }
            catch (Exception)
            {
                throw new Exception("Error al guardar la cuenta");
            }
        }
        public IEnumerable<Account> GetAllAccountsByCliente(long clientId)
        {
            try
            {
                return _accountRepository.GetAccountsByClient(clientId);
            }
            catch (Exception)
            {
                throw new Exception("Error al obtener todos las cuentas");
            }
        }
    }
}
