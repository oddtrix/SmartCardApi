using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.DTOs.Authentication.LogIn;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using System.Security.Claims;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthService authService;

        public AuthenticationController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserSignupDTO userSignupDTO, string role = "User")
        {
            var registerResult = await this.authService.RegisterAsync(userSignupDTO, role);

            if (registerResult.IsSucceed)
            {
                return Ok(registerResult);
            } 

            return BadRequest(registerResult);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var loginResult = await this.authService.LoginAsync(userLoginDTO);

            if (loginResult.IsSucceed)
            {
                return Ok(loginResult);
            }

            return BadRequest(loginResult);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var logoutResult = await this.authService.LogoutAsync();

            if (logoutResult.IsSucceed)
            {
                return Ok(logoutResult);
            }

            return BadRequest(logoutResult);
        }

        [HttpGet]
        public async Task<IActionResult> Me()
        {
            var userNameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString();
            var userId = Guid.Parse(userNameIdentifier);

            var meResult = await this.authService.MeAsync(userId);

            if (meResult.IsSucceed)
            {
                return Ok(meResult);
            }

            return BadRequest(meResult);

        }
    }
}
