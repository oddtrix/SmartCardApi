using SmartCardApi.Models.DTOs.Authentication.LogIn;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using SmartCardApi.Models.DTOs.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponceDTO> RegisterAsync(UserSignupDTO userSignupDTO, string role);

        Task<AuthServiceResponceDTO> LoginAsync(UserLoginDTO userLoginDTO);

        Task<AuthServiceResponceDTO> LogoutAsync();

        Task<AuthServiceResponceDTO> MeAsync(Guid userId);

        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
