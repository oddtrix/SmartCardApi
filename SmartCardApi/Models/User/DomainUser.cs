using SmartCardApi.Models.Cards;

namespace SmartCardApi.Models.User
{
    public class DomainUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}
