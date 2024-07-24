using Domain.Interfaces;
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

        public Card GetById(Guid id)
        {
            return this.context.Cards.FirstOrDefault(card => card.Id == id);
        } 

        public IEnumerable<Card> GetAll()
        {
            return this.context.Cards.ToList();
        }

        public Card Create(Card card, Guid userId)
        {
            card.Id = Guid.Empty;
            card.UserId = userId;
            this.context.Cards.Add(card);
            this.context.SaveChanges();
            return card;
        }

        public void Delete(Guid id)
        {
            this.context.Cards.Remove(new Card { Id = id });
            this.context.SaveChanges();
        }

        public Card Update(Card card)
        {
            this.context.Cards.Update(card);
            this.context.SaveChanges();
            return card;
        }

        public IEnumerable<Card> GetAllByUserId(Guid id)
        {
            return this.context.Cards.Where(c => c.UserId == id).ToList();
        }
    }
}
