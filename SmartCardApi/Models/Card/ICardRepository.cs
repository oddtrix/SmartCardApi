namespace SmartCardApi.Models.Cards
{
    public interface ICardRepository
    {
        IEnumerable<Card> Cards { get; }

        Card this[Guid id] { get; }

        Card Create(Card card, Guid userId);

        Card Update(Card card);

        void Delete(Guid id);
    }
}
