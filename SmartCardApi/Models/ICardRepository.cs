namespace SmartCardApi.Models
{
    public interface ICardRepository
    {
        IEnumerable<Card> Cards { get; }

        Card this[Guid id] { get; }

        Card Create(Card card);

        Card Update(Card card);

        void Delete(Guid id);
    }
}
