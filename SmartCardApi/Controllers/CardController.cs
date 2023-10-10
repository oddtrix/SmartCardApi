using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Card;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CardController : Controller
    {
        private ICardRepository repository;

        private IMapper mapper;

        public CardController(ICardRepository repository, IMapper mapper)
        {
            this.repository = repository;   
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<CardGetDTO>> Get()
        {
            var cards = this.repository.Cards.ToList();
            var convertedCards = mapper.Map<IEnumerable<CardGetDTO>>(cards);

            /*return this.Ok(this.repository.Cards ?? new List<Card>());*/
            return this.Ok(convertedCards);
        }

        [HttpPost]
        public ActionResult<Card> Create([FromBody] CardCreateDTO dto)
        {
            var newCard = mapper.Map<Card>(dto); 
            return Created($"~/api/{nameof(CardController)}/{nameof(this.Create)}", this.repository.Create(newCard));
        }


        [HttpPut]
        public ActionResult<Card> Update([FromBody] CardUpdateDTO dto) 
        {
            var editedCard = mapper.Map<Card>(dto);
            this.repository.Update(editedCard);
            return Ok(editedCard);
        }

        /*[HttpDelete]*/
        [HttpDelete("guid")] // <-- react shit
        public ActionResult Delete([FromBody]CardDeleteDTO dto) 
        {
            var deletedCard = mapper.Map<Card>(dto);
            this.repository.Delete(deletedCard.Id);
            return Ok($"Card with id: {deletedCard.Id} was deleted");
        }

        
    }
}
