using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Card;
using System.Security.Authentication;
using System.Security.Claims;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DomainController : Controller
    {
        private ICardRepository repository;

        private IMapper mapper;

        public DomainController(ICardRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult<Card> Create([FromBody] CardCreateDTO dto)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var newCard = mapper.Map<Card>(dto);
            return Created($"~/api/{nameof(DomainController)}/{nameof(this.Create)}", this.repository.Create(newCard, userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<CardGetDTO>> GetAllCards()
        {
            var cards = this.repository.Cards.ToList();
            var convertedCards = mapper.Map<IEnumerable<CardGetDTO>>(cards);

            return this.Ok(convertedCards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{userId:guid}")]
        public ActionResult<IEnumerable<CardGetDTO>> Get(Guid userId)
        {
            var cards = this.repository.Cards.Where(c => c.UserId == userId).ToList();
            var convertedCards = mapper.Map<IEnumerable<CardGetDTO>>(cards);

            return this.Ok(convertedCards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("guid")] //test 
        public ActionResult<CardGetDTO> GetCardById(Guid cardId) //test
        {
            var card = this.repository.Cards.FirstOrDefault(c => c.Id == cardId);
            var convertedCard = mapper.Map<CardGetDTO>(card);

            return this.Ok(convertedCard);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> Update([FromBody] CardUpdateDTO dto) 
        {
            var editedCard = mapper.Map<Card>(dto);
            editedCard.UserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            this.repository.Update(editedCard);
            return Ok(editedCard);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> IncreaseLearningRate([FromBody] CardIncreaseLearningRateDTO dto)
        {
            Card card = this.repository[dto.Id];
            card.LearningRate += 1;
            if (card.LearningRate == 101) card.LearningRate = 100;
            
            this.repository.Update(card);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> DecreaseLearningRate([FromBody] CardIncreaseLearningRateDTO dto)
        {
            Card card = this.repository[dto.Id];
            card.LearningRate -= 1;
            if(card.LearningRate == -1) card.LearningRate = 0;         
            
            this.repository.Update(card);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete([FromBody] CardDeleteDTO dto)
        {
            var deletedCard = mapper.Map<Card>(dto);
            this.repository.Delete(deletedCard.Id);
            return Ok($"Card with id: {deletedCard.Id} was deleted");
        }
    }
}
