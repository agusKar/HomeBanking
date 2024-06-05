using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
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
        [HttpGet("current")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }

                var clientDTO = new ClientDTO(client);

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
                //validamos datos antes
                if (String.IsNullOrEmpty(newClientDTO.Email) || String.IsNullOrEmpty(newClientDTO.Password) || String.IsNullOrEmpty(newClientDTO.FirstName) || String.IsNullOrEmpty(newClientDTO.LastName))
                    return StatusCode(403, "datos inválidos");

                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(newClientDTO.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }

                Client newClient = new Client
                {
                    Email = newClientDTO.Email,
                    Password = newClientDTO.Password,
                    FirstName = newClientDTO.FirstName,
                    LastName = newClientDTO.LastName,
                };

                _clientRepository.Save(newClient);
                return Created("", newClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //cuenta creada y asignada al cliente autenticado
        [HttpPost("current/accounts")]
        public IActionResult CreateAccountToClientAuthenticated()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * verificar si este cliente tiene menos de 3 cuentas sino, devolver 403
                  * si tiene menos de 3 cuentas crear la cuenta, asignarla al cliente obtenido y guardarla, así como retornar una respuesta “201 creada". tiene que empezar con VIN y seguido de max 8 numeros aleatorios. el saldo de la cuenta tiene que ser 0.
                */
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }

                // traigo todas las cuenta para despues validar si tiene menos de 3
                var accountsByClient = _accountRepository.GetAccountsByClient(client.Id);
                Account accountCreate = null;
                if (accountsByClient.Count() < 3)
                {
                    var flag = 1;
                    var NumberAccountRandom = "";
                    while (flag == 1)
                    {
                        NumberAccountRandom = "VIN-" + RandomNumberGenerator.GetInt32(10000000, 99999999);
                        var getAccount = _accountRepository.GetAccountByNumber(NumberAccountRandom);
                        if (getAccount == null)
                        {
                            flag = 0;
                        }
                    }

                    accountCreate = new Account
                    {
                        Number = NumberAccountRandom.ToString(),
                        Balance = 0,
                        CreationDate = DateTime.Now,
                        ClientId = client.Id
                    };
                }
                else
                {
                    return StatusCode(403, "Este cliente llego al máximo de cuentas.");
                }
                _accountRepository.SaveAccount(accountCreate);
                return StatusCode(201, "La cuenta fue creado exitosamente y fue asignada al cliente correctamente.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        //Traer todas las cuentas del cliente autenticado - JSON con las cuentas de un cliente
        [HttpGet("current/accounts")]
        public IActionResult GetAllAccountsByClient()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * traer todas las cuentas de este cliente
                */
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }

                var accountsByClient = _accountRepository.GetAccountsByClient(client.Id);

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
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }
                // validar de que tipo es y cuantas hay en la base de datos, si son menos de 3 de ese tipo crear una nueva si no devolver 403
                if (_cardRepository.GetAllCardsByType(client.Id, newCardDTO.type).Count() < 3)
                {
                    // numero de cvv de 3 digitos aleatorio
                    var cvvCardRandom = RandomNumberGenerator.GetInt32(100, 999);

                    var flag = 1;
                    var numberCardRandom = "";
                    while (flag == 1)
                    {
                        // numero de tarjeta de 4 digitos aleatorio (repetir en un form)
                        numberCardRandom = "";
                        for (int i = 0; i < 4; i++)
                        {
                            numberCardRandom += RandomNumberGenerator.GetInt32(1000, 9999);
                            if (i < 3)
                            {
                                numberCardRandom += "-";
                            }

                        }
                        var getCard = _cardRepository.GetCardByNumber(client.Id, numberCardRandom);
                        if (getCard == null)
                        {
                            flag = 0;
                        }
                    }

                    var newCard = new Card
                    {
                        ClientId = client.Id,
                        CardHolder = client.FirstName + " " + client.LastName,
                        Color = newCardDTO.color,
                        Cvv = cvvCardRandom,
                        FromDate = DateTime.Now,
                        ThruDate = DateTime.Now.AddYears(5),
                        Number = numberCardRandom,
                        Type = newCardDTO.type
                    };

                    _cardRepository.AddCard(newCard);
                }
                else
                {
                    return StatusCode(403, $"Intentaste crear una tarjeta del tipo {newCardDTO.type}, pero llegaste al limite");
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
        public IActionResult GetAllCardsByClient()
        {
            try
            {
                /* 
                  * traer el cliente autenticado
                  * traer todas las cards de este cliente
                */
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No estas autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no se encontro");
                }

                var cardsByClient = _cardRepository.GetAllCardsByClient(client.Id);

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
