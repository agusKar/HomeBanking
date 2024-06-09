using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return FindAll()
                  .ToList();
        }

        public Transaction GetTransactionsById(long id)
        {
            return FindByCondition(transaction => transaction.Id == id)
                .FirstOrDefault();
        }
        public int SaveTransaction(Transaction transaction)
        {
            Create(transaction);
            return SaveChanges();
        }
    }
}
