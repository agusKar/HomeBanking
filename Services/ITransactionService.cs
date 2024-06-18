using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetAllTransactions();
        Transaction GetTransactionsById(long id);
        int SaveTransaction(Transaction transaction);
        TransferReturnDTO CreateTransaction(TransferDTO transferDTO, long id);
    }
}
