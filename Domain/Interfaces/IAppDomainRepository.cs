using SmartCardApi.Models.User;

namespace Domain.Interfaces
{
    public interface IAppDomainRepository
    {
        void Delete(Guid Id);

        DomainUser GetById(Guid id);

        IEnumerable<DomainUser> GetAll();

        DomainUser Create(DomainUser user);

        DomainUser Update(DomainUser user);
    }
}
