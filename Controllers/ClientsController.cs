using HomeBanking.DTOs;
using HomeBanking.Utilities;
using HomeBanking.Models;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authentication;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IClientService _clientService;
        private readonly ICardService _cardService;

        public ClientsController(
            IAccountService accountService,
            IClientService clientService,
            ICardService cardService
        )
        {
            _accountService = accountService;
            _clientService = clientService;
            _cardService = cardService;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public Client GetCurrentClient()
        {
            string email = User.FindFirst("Client")?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                throw new CustomException("Usuario no encontrado", 403);
            }

            return _clientService.GetClientByEmail(email);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllClients() {
            try
            {
                var clientsDTO = _clientService.GetAllClients();
                return Ok(clientsDTO);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetClientsById(long id) {
            try
            {
                var clientByIdDTO = _clientService.GetClientById(id);
                return Ok(clientByIdDTO);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        
        [HttpGet("current")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetCurrent()
        {
            try
            {
                Client clientCurrent = GetCurrentClient();
                return Ok(new ClientDTO(clientCurrent));
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewClientDTO newClientDTO)
        {
            try
            {
                Client client = _clientService.GetClientByEmail(newClientDTO.Email);
                if (client != null) {
                    throw new CustomException("Usuario ya existe", 403);
                }

                //guardo cliente y retorno ID
                Client ClientCreated = _clientService.Save(newClientDTO);

                // creo cuenta usando el servicio
                _accountService.SaveAccount(ClientCreated.Id);
                ClientDTO ClientDto = new ClientDTO(ClientCreated);

                return StatusCode(201, ClientDto);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        //cuenta creada y asignada al cliente autenticado
        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateAccountToClientAuthenticated()
        {
            try
            {
                Client clientCurrent = GetCurrentClient();

                // traigo todas las cuenta para despues validar si tiene menos de 3
                if (_accountService.GetCountAccountsByClient(clientCurrent.Id) < 3)
                {
                    Account accountCreated = _accountService.SaveAccount(clientCurrent.Id);

                    return StatusCode(201, new AccountClientDTO(accountCreated));
                }
                else
                {
                    return StatusCode(403,"Este cliente llego al máximo de cuentas");
                }
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        
        //Traer todas las cuentas del cliente autenticado - JSON con las cuentas de un cliente
        [HttpGet("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetAllAccountsByClient()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * traer todas las cuentas de este cliente
                */
                Client clientCurrent = GetCurrentClient();

                var accountsByClient = _accountService.GetAllAccountsByCliente(clientCurrent.Id);

                var accountsDto = accountsByClient.Select(a => new AccountClientDTO(a)).ToList();

                return Ok(accountsDto);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        
        //tarjeta creada y asignada al cliente autenticado
        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateCardToClientAuthenticated([FromBody] NewCardDTO newCardDTO)
        {
            try
            {
                Client currentClient = GetCurrentClient();

                Card cardCreated = _cardService.AddCard(currentClient, newCardDTO);

                return Ok(new CardDTO(cardCreated));
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
        
        //Traer todas las cards del cliente autenticado - devuelve JSON con las tarjetas de un cliente
        [HttpGet("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetAllCardsByClient()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * traer todas las cards de este cliente
                */
                Client clientCurrent = GetCurrentClient();

                var cardsByClient = _cardService.GetAllCardsByClient(clientCurrent.Id);

                var cardDto = cardsByClient.Select(c => new CardDTO(c)).ToList();

                return Ok(cardDto);
            }
            catch (CustomException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }
    }
}
