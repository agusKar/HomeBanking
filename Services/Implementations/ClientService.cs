using HomeBanking.DTOs;
using HomeBanking.Utilities;
using HomeBanking.Models;
using HomeBanking.Repositories;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class ClientService: IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client Save(NewClientDTO newClientDTO)
        {
            try
            {
                Client client = new Client
                {
                    Email = newClientDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(newClientDTO.Password),
                    FirstName = newClientDTO.FirstName,
                    LastName = newClientDTO.LastName
                };
                _clientRepository.Save(client);

                return _clientRepository.FindByEmail(client.Email);
            }
            catch (Exception)
            {
                throw new CustomException("Error al guardar y retornar Id de client", HttpStatusCode.Forbidden);
            }
        }
        public Client GetClientByEmail(string email)
        {
            try
            {
                return _clientRepository.FindByEmail(email);
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener client por email", HttpStatusCode.Forbidden);
            }
        }
        public IEnumerable<ClientDTO> GetAllClients()
        {
            try
            {
                IEnumerable<Client> clients = _clientRepository.GetAllClients();
                return clients.Select(c => new ClientDTO(c)).ToList();
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener todos los clients", HttpStatusCode.Forbidden);
            }
        }
        public ClientDTO GetClientById(long id)
        {
            try
            {
                Client clientById = _clientRepository.FindById(id);
                return new ClientDTO(clientById);
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener todos los clients", HttpStatusCode.Forbidden);
            }
        }
    }
}
