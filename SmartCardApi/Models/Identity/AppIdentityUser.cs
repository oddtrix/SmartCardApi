using Microsoft.AspNetCore.Identity;
using SmartCardApi.Models.Cards;

namespace SmartCardApi.Models.Identity
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
