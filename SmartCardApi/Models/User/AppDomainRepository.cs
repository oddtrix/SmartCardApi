using SmartCardApi.Contexts;

namespace SmartCardApi.Models.User
{
    public class AppDomainRepository : IAppDomainRepository
    {
        private AppDomainDbContext context;

        public AppDomainRepository(AppDomainDbContext cartDbContext)
        {
            this.context = cartDbContext;
        }

        public DomainUser this[Guid id] => this.context.Set<DomainUser>().Find(id);

        public IEnumerable<DomainUser> Users => this.context.Set<DomainUser>();

        public DomainUser Create(DomainUser user)
        {
            this.context.Add<DomainUser>(user);
            this.context.SaveChanges();
            return user;
        }

        public void Delete(Guid Id)
        {
            this.context.Remove<DomainUser>(new DomainUser { Id = Id });
            this.context.SaveChanges();
        }

        public DomainUser Update(DomainUser user)
        {
            this.context.Update<DomainUser>(user);
            this.context.SaveChanges();
            return user;
        }
    }
}
