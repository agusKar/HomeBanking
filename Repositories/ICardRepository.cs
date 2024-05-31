using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card GetCardById(long id);
        void AddCard(Card card);
    }
}
