using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ICardService
    {
        int GenerateCvv();
        string GenerateNumberCardUnique(long clientId);
        IEnumerable<Card> GetAllCardsByType(long clientId, string type);
        IEnumerable<Card> GetAllCardsByClient(long clientId);
        void AddCard(Card card);
    }
}
