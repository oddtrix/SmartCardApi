using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SmartCardApi.Models.DTOs.Authentication.LogIn;
using SmartCardApi.Models.DTOs.Authentication.SignUp;
using SmartCardApi.Models.Identity;
using SmartCardApi.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartCardApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<AppIdentityUser> userManager;

        private RoleManager<IdentityRole<Guid>> roleManager;

        private SignInManager<AppIdentityUser> signInManager;

        private IAppDomainRepository domainRepository;

        private IConfiguration configuration;

        private IMapper mapper;

        public AuthenticationController(UserManager<AppIdentityUser> userManager,
                                        RoleManager<IdentityRole<Guid>> roleManager,
                                        SignInManager<AppIdentityUser> signInManager,
                                        IAppDomainRepository domainRepository,
                                        IConfiguration configuration,
                                        IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.domainRepository = domainRepository;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserSignupDTO userSignupDTO, string role = "User")
        {
            var userExist = await userManager.FindByEmailAsync(userSignupDTO.Email);
            if (userExist != null) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Status = "Error", message = "User already exists"});
            }

            var newUser = mapper.Map<AppIdentityUser>(userSignupDTO);
            
            if(await roleManager.RoleExistsAsync(role))
            {
                IdentityResult result = await userManager.CreateAsync(newUser, userSignupDTO.Password);
                
                var addedUser = await userManager.FindByEmailAsync(newUser.Email);
                var newDomainUser = mapper.Map<DomainUser>(addedUser);
                this.domainRepository.Create(newDomainUser);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            result.Errors.Select(e => e.Description));
                }
                
                await userManager.AddToRoleAsync(newUser, role);

                return StatusCode(StatusCodes.Status200OK,
                            new { Status = "Success", message = "User created successfully." });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new { Status = "Error", message = "This role doen`t exist."});
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
                };

                var userRoles = await userManager.GetRolesAsync(user);
                authClaim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                foreach (var role in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = GetToken(authClaim);

                await signInManager.SignOutAsync();

                var signInResult = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                }
            }

            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Me()
        {
            var userNameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString();
            var userId = Guid.Parse(userNameIdentifier);
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                return Ok(userId);
            }

            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(authSignInToken, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
