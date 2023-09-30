using Microsoft.AspNetCore.Mvc;
using SmartCardApi.Models;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private ICardRepository repository;

        public CardController(ICardRepository repository)
        {
            this.repository = repository;   
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            return repository.Cards;
        }
    }
}
