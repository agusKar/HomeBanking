using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public void AddCard(Card card)
        {
            Create(card);
            SaveChanges();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return FindAll()
                .ToList();
        }

        public Card GetCardById(long id)
        {
            return FindByCondition(c => c.Id == id)
                .FirstOrDefault();
        }
    }
}
