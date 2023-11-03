using Microsoft.AspNetCore.Identity;

namespace SmartCardApi.Models.Identity
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
