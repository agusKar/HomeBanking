using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAccountService
    {
        string GetRandomAccountNumber();
        int GetCountAccountsByClient(long clientId);
        void SaveAccount(Account account);
        IEnumerable<Account> GetAllAccountsByCliente(long clientId);
    }
}
