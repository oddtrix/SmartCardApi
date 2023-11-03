using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Card;
using System.Security.Claims;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DomainController : Controller
    {
        private ICardService cardService;

        public DomainController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult<Card> Create([FromBody] CardCreateDTO dto)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var newCard = this.cardService.CreateCard(dto, userId);
            return Created($"~/api/{nameof(DomainController)}/{nameof(this.Create)}", newCard);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<CardGetDTO>> GetAllCards()
        {
            var cards = this.cardService.GetAllCards();
            return this.Ok(cards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{userId:guid}")]
        public ActionResult<IEnumerable<CardGetDTO>> Get(Guid userId)
        {
            var cards = this.cardService.GetCardsByUserId(userId);
            return this.Ok(cards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("guid")]
        public ActionResult<CardGetDTO> GetCardById(Guid cardId)
        {
            var card = this.cardService.GetCardById(cardId);
            return this.Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> Update([FromBody] CardUpdateDTO dto) 
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var editedCard = this.cardService.UpdateCard(dto, userId);
            return Ok(editedCard);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> IncreaseLearningRate([FromBody] CardIncreaseLearningRateDTO dto)
        {
            var card = this.cardService.IncreaseLearningRate(dto.Id);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> DecreaseLearningRate([FromBody] CardIncreaseLearningRateDTO dto)
        {
            var card = this.cardService.DecreaseLearningRate(dto.Id);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete([FromBody] CardDeleteDTO dto)
        {
            this.cardService.DeleteCard(dto.Id);
            return Ok($"Card with id: {dto.Id} was deleted");
        }
    }
}
