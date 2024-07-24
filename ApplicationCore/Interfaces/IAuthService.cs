using Domain.DTOs.Authentication.LogIn;
using Domain.DTOs.Authentication.SignUp;
using Domain.DTOs.Services;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponceDTO> LogoutAsync();

        Task<AuthServiceResponceDTO> MeAsync(Guid userId);

        Task<AuthServiceResponceDTO> LoginAsync(UserLoginDTO userLoginDTO);

        Task<AuthServiceResponceDTO> RegisterAsync(UserSignupDTO userSignupDTO, string role);
    }
}
