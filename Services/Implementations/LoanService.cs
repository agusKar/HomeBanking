using HomeBanking.Models;
using HomeBanking.Repositories;

namespace HomeBanking.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public IEnumerable<Loan> GetAllLoans()
        {
			try
			{
                return _loanRepository.GetAll();
            }
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
        }
        public Loan GetLoanById(long loanId)
        {
            try
            {
                return _loanRepository.GetLoanById(loanId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
