using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAccountService
    {
        string GetRandomAccountNumber();
        int GetCountAccountsByClient(long clientId);
        Account GetAccountByNumber(string numberAccount);
        void SaveAccount(Account account);
        IEnumerable<Account> GetAllAccountsByCliente(long clientId);
        IEnumerable<Account> GetAllAccounts();
        Account GetAccountById(long id);

    }
}
