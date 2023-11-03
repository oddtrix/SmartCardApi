using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Card;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface ICardService
    {
        Card CreateCard(CardCreateDTO dto, Guid userId);

        IEnumerable<CardGetDTO> GetAllCards();

        IEnumerable<CardGetDTO> GetCardsByUserId(Guid userId);

        CardGetDTO GetCardById(Guid cardId);

        Card UpdateCard(CardUpdateDTO dto, Guid userId);

        Card IncreaseLearningRate(Guid cardId);

        Card DecreaseLearningRate(Guid cardId);

        void DeleteCard(Guid cardId);
    }
}
