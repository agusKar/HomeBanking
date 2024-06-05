using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientService
    {
        long SaveAndReturnIdClient(Client client);
        Client GetClientByEmail(string email);
        Client GetClientById(long id);
        IEnumerable<Client> GetAllClients();
    }
}
