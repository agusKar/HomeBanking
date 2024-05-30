using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan GetLoanById(long id);
        void SaveLoan(Loan loan);
    }
}
