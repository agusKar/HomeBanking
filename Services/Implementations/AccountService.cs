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
                throw new CustomException("Error getting account by number.", 403);
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
                throw new CustomException("Error getting all accounts.", 403);
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
                throw new CustomException("Error saving the account.", 403);
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
                throw new CustomException("Error modifing account.", 403);
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
                throw new CustomException("Error getting all accounts by client ID.", 403);
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
                throw new CustomException("Error getting account by ID.", 403);
            }
        }
    }
}
