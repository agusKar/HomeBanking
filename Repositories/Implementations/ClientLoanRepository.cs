using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
            
        }

        public IEnumerable<ClientLoan> GetAllClientsLoans()
        {
            return FindAll()
                 .Include(c => c.Client)
                 .Include(c => c.Loan)
                .ToList();
        }

        public ClientLoan GetClientLoanById(long id)
        {
            return FindByCondition(c => c.Id == id)
                .FirstOrDefault();
        }

        public void SaveClientLoan(ClientLoan clientLoan)
        {
            Create(clientLoan);
            SaveChanges();
        }
    }
}
