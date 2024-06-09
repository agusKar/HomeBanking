using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAccountService
    {
        string GetRandomAccountNumber();
        int GetCountAccountsByClient(long clientId);
        Account GetAccountByNumber(string numberAccount);
        Account SaveAccount(long newIdCreated);
        void UpdateAccount(Account account);
        IEnumerable<Account> GetAllAccountsByCliente(long clientId);
        IEnumerable<Account> GetAllAccounts();
        Account GetAccountById(long id);

    }
}
