using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface IClientLoanRepository
    {
        void SaveClientLoan(ClientLoan clientLoan);
    }
}
