namespace SmartCardApi.Models
{
    public class CardRepository : ICardRepository
    {
        private CarDDbContext context;

        public CardRepository(CarDDbContext cartDbContext)
        {
            context = cartDbContext;
        }

        public Card this[Guid id] => context.Cards.First(x => x.Id == id);

        public IEnumerable<Card> Cards => context.Cards;

        public Card Create(Card card)
        {
            context.Cards.Add(card);
            context.SaveChanges();
            return card;
        }

        public void Delete(Guid id)
        {
            context.Cards.Remove(new Card { Id = id });
            context.SaveChanges();
        }

        public Card Update(Card card)
        {
            context.Cards.Update(card);
            context.SaveChanges();
            return card;
        }
    }
}
