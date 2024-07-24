using Domain.Interfaces;
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

        public DomainUser GetById(Guid id)
        {
            return this.context.DomainUsers.FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<DomainUser> GetAll()
        {
            return this.context.DomainUsers.ToList();
        }

        public DomainUser Create(DomainUser user)
        {
            this.context.DomainUsers.Add(user);
            this.context.SaveChanges();
            return user;
        }

        public void Delete(Guid Id)
        {
            this.context.DomainUsers.Remove(new DomainUser { Id = Id });
            this.context.SaveChanges();
        }

        public DomainUser Update(DomainUser user)
        {
            this.context.DomainUsers.Update(user);
            this.context.SaveChanges();
            return user;
        }
    }
}
