using SmartCardApi.BusinessLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApplicationCore.Interfaces
{
    public interface IJwtAuthService : IAuthService
    {
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
