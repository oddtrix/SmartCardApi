using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using SmartCardApi.Models.Identity;
using SmartCardService.Models;
using SmartCardService.Services;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<AppUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private IEmailService emailService;

        private IConfiguration configuration;

        private IMapper mapper;

        public AuthenticationController(UserManager<AppUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IEmailService emailService,
                                        IConfiguration configuration,
                                        IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserSignupDTO userSignupDTO, string role)
        {
            var userExist = await userManager.FindByEmailAsync(userSignupDTO.Email);
            if (userExist != null) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Status = "Error", message = "User already exists"});
            }

            var newUser = mapper.Map<AppUser>(userSignupDTO);

            if(await roleManager.RoleExistsAsync(role))
            {
                IdentityResult result = await userManager.CreateAsync(newUser, userSignupDTO.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            result.Errors.Select(e => e.Description));
                }
                
                await userManager.AddToRoleAsync(newUser, role);

                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                string http = configuration["Jwt:Issuer"];
                var confirmLink = http.Substring(0, http.Length - 1) + Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = newUser.Email });
                var message = new Message(new string[] { newUser.Email! }, "Confirmation email link", confirmLink);
                emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
                            new { Status = "Success", message = "User created successfully." });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new { Status = "Error", message = "This role doen`t exist."});
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        Status = "Success",
                        message = "Email has been confirmed"
                    });
                }  
            }   
            return StatusCode(StatusCodes.Status500InternalServerError,
                            new { Status = "Error", message = "This user doesn`t exist" });
        }
    }
}
