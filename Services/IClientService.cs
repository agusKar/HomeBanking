using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientService
    {
        Client Save(NewClientDTO newClientDTO);
        Client GetClientByEmail(string email);
        ClientDTO GetClientById(long id);
        IEnumerable<ClientDTO> GetAllClients();
    }
}
