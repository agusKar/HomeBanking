using HomeBanking.Models;
using HomeBanking.Repositories;

namespace HomeBanking.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public IEnumerable<Transaction> GetAllTransactions()
        {
            try
            {
                return _transactionRepository.GetAllTransactions();
            }
            catch (Exception)
            {
                throw new Exception("Error al traer todas las transacciones.");
            }
        }

        public Transaction GetTransactionsById(long id)
        {
            try
            {
                return _transactionRepository.GetTransactionsById(id);
            }
            catch (Exception)
            {
                throw new Exception("Error al traer la transaccion por id.");
            }
        }

        public void SaveTransaction(Transaction transaction)
        {
            try
            {
                _transactionRepository.SaveTransaction(transaction);
            }
            catch (Exception)
            {
                throw new Exception("Error al guardar la transaccion.");
            }
        }
    }
}
