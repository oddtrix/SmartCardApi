using Domain.DTOs.Card;
using SmartCardApi.Models.Cards;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface ICardCoreService
    {
        void DeleteCard(Guid cardId);

        CardGetDTO GetCardById(Guid cardId);

        IEnumerable<CardGetDTO> GetAllCards();

        Card UpdateCard(CardUpdateDTO dto, Guid userId);

        Card CreateCard(CardCreateDTO dto, Guid userId);
    }
}
