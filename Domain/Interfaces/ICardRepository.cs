using SmartCardApi.Models.Cards;

namespace Domain.Interfaces
{
    public interface ICardRepository
    {
        void Delete(Guid id);

        Card GetById(Guid id);

        Card Update(Card card);

        IEnumerable<Card> GetAll();

        Card Create(Card card, Guid userId);

        IEnumerable<Card> GetAllByUserId(Guid id);
    }
}
