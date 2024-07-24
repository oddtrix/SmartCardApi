using ApplicationCore.Interfaces;
using Domain.DTOs.Authentication.LogIn;
using Domain.DTOs.Authentication.SignUp;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartCardApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IJwtAuthService authService;

        public AuthenticationController(IJwtAuthService authService)
        {
            this.authService = authService;
        }

        [Route("register")]
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

        [Route("login")]
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

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var logoutResult = await this.authService.LogoutAsync();

            if (logoutResult.IsSucceed)
            {
                return Ok(logoutResult);
            }

            return BadRequest(logoutResult);
        }

        [Route("me")]
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
