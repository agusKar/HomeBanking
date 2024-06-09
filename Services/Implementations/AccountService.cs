using HomeBanking.Utilities;
using HomeBanking.Models;
using HomeBanking.Repositories;
using System.Security.Cryptography;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public Account GetAccountByNumber(string numberAccount)
        {
            try
            {
                return _accountRepository.GetAccountByNumber(numberAccount);
            }
            catch (Exception)
            {
                throw new CustomException("Error al traer la cuenta por su numero.", HttpStatusCode.Forbidden);
            }
        }
        public int GetCountAccountsByClient(long clientId)
        {
            var accountsByClient = _accountRepository.GetAccountsByClient(clientId);
            return accountsByClient.Count();
        }
        public IEnumerable<Account> GetAllAccounts()
        {
            try
            {
                return _accountRepository.GetAllAccounts();
            }
            catch (Exception)
            {
                throw new CustomException("Error al traer todas las cuentas.", HttpStatusCode.Forbidden);
            }
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
        public Account SaveAccount(long newIdCreated)
        {
            try
            {
                Account account = new Account
                {
                    Number = GetRandomAccountNumber().ToString(),
                    Balance = 0,
                    CreationDate = DateTime.Now,
                    ClientId = newIdCreated
                };
                _accountRepository.SaveAccount(account);

                return _accountRepository.GetAccountByNumber(account.Number);
            }
            catch (Exception)
            {
                throw new CustomException("Error al guardar la cuenta.", HttpStatusCode.Forbidden);
            }
        }
        public void UpdateAccount(Account account)
        {
            try
            {
                _accountRepository.SaveAccount(account);
            }
            catch (Exception)
            {
                throw new CustomException("Error al modificar la cuenta.", HttpStatusCode.Forbidden);
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
                throw new CustomException("Error al obtener todos las cuentas.", HttpStatusCode.Forbidden);
            }
        }
        public Account GetAccountById(long id)
        {
            try
            {
                return _accountRepository.GetAccountById(id);
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener la cuenta por ID.", HttpStatusCode.Forbidden);
            }
        }
    }
}
