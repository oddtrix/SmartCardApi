using Microsoft.AspNetCore.Identity;
using SmartCardApi.Models.Cards;

namespace SmartCardApi.Models.Identity
{
    public class AppUser : IdentityUser
    {
        /*public IEnumerable<Card> Cards { get; set; }  */

        public string Name { get; set; }

        public string Surname { get; set; } 
    }
}
