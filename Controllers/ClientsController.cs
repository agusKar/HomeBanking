using HomeBanking.DTOs;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult GetAllClients() {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = clients.Select(c => new ClientDTO(c)).ToList();

                return Ok(clientsDTO);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetClientsById(long id) {
            try
            {
                var clientById = _clientRepository.FindById(id);
                var clientByIdDTO = new ClientDTO(clientById);
                return Ok(clientByIdDTO);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
