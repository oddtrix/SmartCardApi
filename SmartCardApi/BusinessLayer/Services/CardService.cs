using AutoMapper;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Card;

namespace SmartCardApi.BLL.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository repository;
        private readonly IMapper mapper;

        public CardService(ICardRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Card CreateCard(CardCreateDTO dto, Guid userId)
        {
            var newCard = mapper.Map<Card>(dto);
            return this.repository.Create(newCard, userId);
        }

        public IEnumerable<CardGetDTO> GetAllCards()
        {
            var cards = this.repository.Cards.ToList();
            return mapper.Map<IEnumerable<CardGetDTO>>(cards);
        }

        public IEnumerable<CardGetDTO> GetCardsByUserId(Guid userId)
        {
            var cards = this.repository.Cards.Where(c => c.UserId == userId).ToList();
            return mapper.Map<IEnumerable<CardGetDTO>>(cards);
        }

        public CardGetDTO GetCardById(Guid cardId)
        {
            var card = this.repository.Cards.FirstOrDefault(c => c.Id == cardId);
            return mapper.Map<CardGetDTO>(card);
        }

        public Card UpdateCard(CardUpdateDTO dto, Guid userId)
        {
            var editedCard = mapper.Map<Card>(dto);
            editedCard.UserId = userId;
            this.repository.Update(editedCard);
            return editedCard;
        }

        public Card IncreaseLearningRate(Guid cardId)
        {
            Card card = this.repository[cardId];
            card.LearningRate += 1;
            if (card.LearningRate == 101) card.LearningRate = 100;
            this.repository.Update(card);
            return card;
        }

        public Card DecreaseLearningRate(Guid cardId)
        {
            Card card = this.repository[cardId];
            card.LearningRate -= 1;
            if (card.LearningRate == -1) card.LearningRate = 0;
            this.repository.Update(card);
            return card;
        }

        public void DeleteCard(Guid cardId)
        {
            this.repository.Delete(cardId);
        }
    }
}
