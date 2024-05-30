using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                .ThenInclude(clientLoan => clientLoan.Loan)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                .ThenInclude(clientLoan => clientLoan.Loan)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
