using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext){ }
        public Account GetAccountById(long id)
        {
            return FindByCondition(account =>  account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }
    }
}
