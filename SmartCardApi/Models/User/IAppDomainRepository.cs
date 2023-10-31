namespace SmartCardApi.Models.User
{
    public interface IAppDomainRepository
    {
        IEnumerable<DomainUser> Users { get; }

        DomainUser this[Guid id] { get; }

        DomainUser Create(DomainUser user);

        DomainUser Update(DomainUser user);

        void Delete(Guid Id);
    }
}
