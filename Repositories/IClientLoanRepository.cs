using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface IClientLoanRepository
    {
        IEnumerable<ClientLoan> GetAllClientsLoans();
        ClientLoan GetClientLoanById(long id);
        void SaveClientLoan(ClientLoan clientLoan);
    }
}
