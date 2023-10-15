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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<CardGetDTO>> GetAllCards()
        {
            var cards = this.repository.Cards.ToList();
            var convertedCards = mapper.Map<IEnumerable<CardGetDTO>>(cards);

            /*return this.Ok(this.repository.Cards ?? new List<Card>());*/
            return this.Ok(convertedCards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet()]
        public ActionResult<IEnumerable<CardGetDTO>> Get()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var cards = this.repository.Cards.Where(c => c.UserId == userId).ToList();
            var convertedCards = mapper.Map<IEnumerable<CardGetDTO>>(cards);

            /*return this.Ok(this.repository.Cards ?? new List<Card>());*/
            return this.Ok(convertedCards);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("guid")] //test 
        public ActionResult<CardGetDTO> GetCardById(Guid cardId) //test
        {
            var card = this.repository.Cards.FirstOrDefault(c => c.Id == cardId);
            var convertedCard = mapper.Map<CardGetDTO>(card);

            /*return this.Ok(this.repository.Cards ?? new List<Card>());*/
            return this.Ok(convertedCard);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult<Card> Create([FromBody] CardCreateDTO dto)
        {
            /*if (!User.Identity.IsAuthenticated)
                return StatusCode(StatusCodes.Status401Unauthorized);*/

            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var newCard = mapper.Map<Card>(dto); 
            return Created($"~/api/{nameof(DomainController)}/{nameof(this.Create)}", this.repository.Create(newCard, userId));
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
        /*[HttpDelete]*/
        [HttpDelete("guid")] // <-- react shit
        public ActionResult Delete([FromBody]CardDeleteDTO dto) 
        {
            var deletedCard = mapper.Map<Card>(dto);
            this.repository.Delete(deletedCard.Id);
            return Ok($"Card with id: {deletedCard.Id} was deleted");
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult<Card> IncreaseLearningRate([FromBody] CardIncreaseLearningRateDTO dto)
        {
            var card = this.GetCardById(dto.Id);
            
            
            Card userCard = mapper.Map<Card>(((OkObjectResult)card.Result).Value);
            userCard.LearningRate += 1;
            userCard.UserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            this.repository.Update(userCard);
            return Ok(userCard);
        }
    }
}
