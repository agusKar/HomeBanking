using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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

        public long SaveAndReturnIdClient(Client client)
        {
            try
            {
                _clientRepository.Save(client);
                Client newClientSaved = _clientRepository.FindByEmail(client.Email);
                return newClientSaved.Id;
            }
            catch (Exception e)
            {
                throw new Exception("Error al guardar y retornar Id de client");
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
                throw new Exception("Error al obtener client por email");
            }
        }
        public IEnumerable<Client> GetAllClients()
        {
            try
            {
                return _clientRepository.GetAllClients();
            }
            catch (Exception)
            {
                throw new Exception("Error al obtener todos los clients");
            }
        }
        public Client GetClientById(long id)
        {
            try
            {
                return _clientRepository.FindById(id);
            }
            catch (Exception)
            {
                throw new Exception("Error al obtener todos los clients");
            }
        }
    }
}
