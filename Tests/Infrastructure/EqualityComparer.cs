using SmartCardApi.Models.User;
using System.Diagnostics.CodeAnalysis;

namespace Tests.Infrastructure
{
    internal class DomainUserEqualityComparer : IEqualityComparer<DomainUser>
    {
        public bool Equals([AllowNull] DomainUser x, [AllowNull] DomainUser y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.UserName == y.UserName
                && x.Surname == y.Surname
                && x.Name == y.Name
                && x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] DomainUser obj)
        {
            return obj.GetHashCode();
        }
    }

}
