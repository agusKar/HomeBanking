using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ILoanService
    {
        IEnumerable<Loan> GetAllLoans();
        Loan GetLoanById(long loanId);
    }
}
