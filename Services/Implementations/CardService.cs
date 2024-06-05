using HomeBanking.Models;
using HomeBanking.Repositories;
using System.Security.Cryptography;

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
                throw new Exception("Error al obtener todas las cards por client y tipo");
            }
        }
        public void AddCard(Card card)
        {
            try
            {
                _cardRepository.AddCard(card);
            }
            catch (Exception)
            {
                throw new Exception("Error al guarda la card");
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
                throw new Exception("Error al guarda la card");
            }
        }
    }
}
