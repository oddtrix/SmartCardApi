using ApplicationCore.Interfaces;
using Domain.DTOs.Card;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.Models.Cards;
using System.Security.Claims;

namespace SmartCardApi.Controllers
{
    [Route("api/cards")]
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
        [HttpGet("user/{userId:guid}")]
        public ActionResult<IEnumerable<CardGetDTO>> GetCardsByUserId([FromRoute] Guid userId)
        {
            var cards = this.cardService.GetCardsByUserId(userId);
            return this.Ok(cards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{cardId:guid}")]
        public ActionResult<CardGetDTO> GetCardById([FromRoute] Guid cardId)
        {
            var card = this.cardService.GetCardById(cardId);
            return this.Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{cardId:guid}")]
        public ActionResult<Card> Update([FromRoute] Guid cardId, [FromBody] CardCreateDTO dto) 
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var updateDto = new CardUpdateDTO { Id = cardId, Translation = dto.Translation, Word = dto.Word };
            var editedCard = this.cardService.UpdateCard(updateDto, userId);
            return Ok(editedCard);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPatch("{cardId:guid}/increaselearningrate")]
        public ActionResult<Card> IncreaseLearningRate([FromRoute] Guid cardId)
        {
            var card = this.cardService.IncreaseLearningRate(cardId);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPatch("{cardId:guid}/decreaselearningrate")]
        public ActionResult<Card> DecreaseLearningRate([FromRoute] Guid cardId)
        {
            var card = this.cardService.DecreaseLearningRate(cardId);
            return Ok(card);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{cardId:guid}")]
        public ActionResult Delete([FromRoute] Guid cardId)
        {
            this.cardService.DeleteCard(cardId);
            return Ok($"Card with id: {cardId} was deleted");
        }
    }
}
