using HomeBanking.Utilities;
using HomeBanking.Models;
using HomeBanking.Repositories;
using System.Net;
using System.Security.Cryptography;
using HomeBanking.DTOs;

namespace HomeBanking.Services.Implementations
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public int GenerateCvv()
        {
            return RandomNumberGenerator.GetInt32(100, 999);
        }

        public string GenerateNumberCardUnique(long clientId)
        {
            var numberCardRandom = "";
            do
            {
                for (int i = 0; i < 4; i++)
                {
                    numberCardRandom += RandomNumberGenerator.GetInt32(1000, 9999);
                    if (i < 3)
                    {
                        numberCardRandom += "-";
                    }
                }
            } while (_cardRepository.GetCardByNumber(clientId, numberCardRandom) != null);

            return numberCardRandom;
        }
        public IEnumerable<Card> GetAllCardsByType(long clientId, string type)
        {
            try
            {
                return _cardRepository.GetAllCardsByType(clientId,type);
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener todas las cards por client y tipo", HttpStatusCode.Forbidden);
            }
        }
        public IEnumerable<Card> GetAllCardsByClient(long clientId)
        {
            try
            {
                return _cardRepository.GetAllCardsByClient(clientId);
            }
            catch (Exception)
            {
                throw new CustomException("Error al traer todas las card por el id del client", HttpStatusCode.Forbidden);
            }
        }
        public Card AddCard(Client currentClient, NewCardDTO newCardDTO)
        {
            try
            {
                // validar de que tipo es y cuantas hay en la base de datos, si son menos de 3 de ese tipo crear una nueva si no devolver
                var cardByClient = GetAllCardsByType(currentClient.Id, newCardDTO.type);
                // hacer un metodo para crear la card y crear una clase de customException y hacer throw new customException
                if (cardByClient.Count() < 3)
                {
                    if (!cardByClient.Any(c => c.Color == newCardDTO.color))
                    {
                        var newCard = new Card
                        {
                            ClientId = currentClient.Id,
                            CardHolder = currentClient.FirstName + " " + currentClient.LastName,
                            Color = newCardDTO.color,
                            Cvv = GenerateCvv(),
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(5),
                            Number = GenerateNumberCardUnique(currentClient.Id),
                            Type = newCardDTO.type
                        };

                        _cardRepository.AddCard(newCard);
                        return _cardRepository.GetCardByNumber(currentClient.Id, newCard.Number);
                    }
                    else
                    {
                        throw new CustomException($"Intentaste crear una tarjeta {newCardDTO.color} del tipo {newCardDTO.type}, pero llegaste al limite.", HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    throw new CustomException($"Intentaste crear una tarjeta del tipo {newCardDTO.type}, pero llegaste al limite.", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message, HttpStatusCode.Forbidden);
            }
        }
    }
}
