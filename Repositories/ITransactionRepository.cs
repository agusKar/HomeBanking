using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        Transaction GetTransactionsById(long id);
        void SaveTransaction(Transaction transaction);
    }
}
