using Domain.DTOs.Card;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.Cards;

namespace ApplicationCore.Interfaces
{
    public interface ICardService : ICardCoreService
    {
        Card IncreaseLearningRate(Guid cardId);

        Card DecreaseLearningRate(Guid cardId);

        IEnumerable<CardGetDTO> GetCardsByUserId(Guid userId);
    }
}
