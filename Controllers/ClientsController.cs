using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public Client GetCurrentClient()
        {
            string email = User.FindFirst("Client")?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("user not found");
            }

            Client client = _clientService.GetClientByEmail(email);

            return client;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllClients() {
            try
            {
                var clients = _clientService.GetAllClients();
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
                var clientById = _clientService.GetClientById(id);
                var clientByIdDTO = new ClientDTO(clientById);
                return Ok(clientByIdDTO);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("current")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrent()
        {
            try
            {
                Client clientCurrent = GetCurrentClient();

                var clientDTO = new ClientDTO(clientCurrent);

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] NewClientDTO newClientDTO)
        {
            try
            {
                Client client = _clientService.GetClientByEmail(newClientDTO.Email);
                if (client != null) {
                    return StatusCode(403, "Usuario ya existe.");
                }

                Client newClient = new Client
                {
                    Email = newClientDTO.Email,
                    Password = newClientDTO.Password,
                    FirstName = newClientDTO.FirstName,
                    LastName = newClientDTO.LastName
                };

                //guardo cliente y retorno ID
                long newIdCreated = _clientService.SaveAndReturnIdClient(newClient);

                // creo cuenta usando el servicio
                Account accountCreate = new Account
                {
                    Number = _accountService.GetRandomAccountNumber().ToString(),
                    Balance = 0,
                    CreationDate = DateTime.Now,
                    ClientId = newIdCreated
                };
                _accountService.SaveAccount(accountCreate);

                return Created();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //cuenta creada y asignada al cliente autenticado
        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateAccountToClientAuthenticated()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * verificar si este cliente tiene menos de 3 cuentas sino, devolver 403
                  * si tiene menos de 3 cuentas crear la cuenta, asignarla al cliente obtenido y guardarla, así como retornar una respuesta “201 creada". tiene que empezar con VIN y seguido de max 8 numeros aleatorios. el saldo de la cuenta tiene que ser 0.
                */
                Client clientCurrent = GetCurrentClient();


                // traigo todas las cuenta para despues validar si tiene menos de 3

                Account accountCreate = null;
                if (_accountService.GetCountAccountsByClient(clientCurrent.Id) < 3)
                {   
                    accountCreate = new Account
                    {
                        Number = _accountService.GetRandomAccountNumber().ToString(),
                        Balance = 0,
                        CreationDate = DateTime.Now,
                        ClientId = clientCurrent.Id
                    };
                }
                else
                {
                    return StatusCode(403, "Este cliente llego al máximo de cuentas.");
                }
                _accountService.SaveAccount(accountCreate);
                return StatusCode(201, "La cuenta fue creado exitosamente y fue asignada al cliente correctamente.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        //tarjeta creada y asignada al cliente autenticado
        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateCardToClientAuthenticated([FromBody] NewCardDTO newCardDTO)
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * verificar que tenga menos de 6, 3 credito - 3 debito, sino devuelve 403
                  * si tiene menos de la condicion crear la tarjeta y asignarla al cliente autenticado.
                  * La fecha de vencimiento debera ser 5 años despues de la creación.
                  * También ten en cuenta que los numeros de tarjeta y el cvv se deben generar de forma aleatoria.
                */
                Client clientCurrent = GetCurrentClient();

                // validar de que tipo es y cuantas hay en la base de datos, si son menos de 3 de ese tipo crear una nueva si no devolver
                var cardByClient = _cardService.GetAllCardsByType(clientCurrent.Id, newCardDTO.type);
                if (cardByClient.Count() < 3)
                {
                    if(!cardByClient.Any(c => c.Color == newCardDTO.color))
                    {
                        var newCard = new Card
                        {
                            ClientId = clientCurrent.Id,
                            CardHolder = clientCurrent.FirstName + " " + clientCurrent.LastName,
                            Color = newCardDTO.color,
                            Cvv = _cardService.GenerateCvv(),
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(5),
                            Number = _cardService.GenerateNumberCardUnique(clientCurrent.Id),
                            Type = newCardDTO.type
                        };

                        _cardService.AddCard(newCard);
                    }
                    else
                    {
                        return StatusCode(403, $"Intentaste crear una tarjeta {newCardDTO.color} del tipo {newCardDTO.type}, pero llegaste al limite.");
                    }
                }
                else
                {
                    return StatusCode(403, $"Intentaste crear una tarjeta del tipo {newCardDTO.type}, pero llegaste al limite.");
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
