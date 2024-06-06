using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetAllTransactions();
        Transaction GetTransactionsById(long id);
        void SaveTransaction(Transaction transaction);
    }
}
