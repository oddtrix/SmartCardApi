using Microsoft.EntityFrameworkCore;
using SmartCardApi.Contexts;

namespace SmartCardApi.Models.Cards
{
    public class CardRepository : ICardRepository
    {
        private AppDomainDbContext context;

        public CardRepository(AppDomainDbContext cartDbContext)
        {
            this.context = cartDbContext;
        }

        public Card this[Guid id] => context.Cards.Find(id);

        public IEnumerable<Card> Cards => context.Cards;

        public Card Create(Card card, Guid userId)
        {
            card.Id = Guid.Empty;
            card.UserId = userId;
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
