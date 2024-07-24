using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
