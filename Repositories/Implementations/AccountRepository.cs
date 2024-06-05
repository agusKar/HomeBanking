using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext){ }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }

        public Account GetAccountById(long id)
        {
            return FindByCondition(account =>  account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }
        public Account GetAccountByNumber(string numberAccount)
        {
            return FindByCondition(account => account.Number == numberAccount)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }
        public IEnumerable<Account> GetAllAccountsByCliente(long clientId)
        {
            return FindByCondition(a => a.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }
        public void SaveAccount(Account account) { 
            Create(account);
            SaveChanges();
        }
    }
}
