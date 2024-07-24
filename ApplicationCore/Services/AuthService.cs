using ApplicationCore.Interfaces;
using AutoMapper;
using Domain.DTOs.Authentication.LogIn;
using Domain.DTOs.Authentication.SignUp;
using Domain.DTOs.Services;
using Domain.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartCardApi.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartCardApi.BusinessLayer.Services
{
    public class AuthService : IJwtAuthService
    {
        private SignInManager<AppIdentityUser> signInManager;

        private RoleManager<IdentityRole<Guid>> roleManager;

        private UserManager<AppIdentityUser> userManager;

        private IAppDomainRepository domainRepository;

        private IConfiguration configuration;

        private IMapper mapper;

        public AuthService(SignInManager<AppIdentityUser> signInManager,
                            RoleManager<IdentityRole<Guid>> roleManager,
                            UserManager<AppIdentityUser> userManager,
                            IAppDomainRepository domainRepository,
                            IConfiguration configuration,
                            IMapper mapper)
        {
            this.domainRepository = domainRepository;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<AuthServiceResponceDTO> LoginAsync(UserLoginDTO userLoginDTO)
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
                authClaim.Add(new Claim(ClaimTypes.Name, user.UserName.ToString()));
                foreach (var role in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = this.GetToken(authClaim);

                await signInManager.SignOutAsync();

                var signInResult = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return new AuthServiceResponceDTO
                    {
                        IsSucceed = true,
                        Message = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    };
                }
            }

            return new AuthServiceResponceDTO { IsSucceed = false, Message = "User not found" };
        }

        public async Task<AuthServiceResponceDTO> RegisterAsync(UserSignupDTO userSignupDTO, string role)
        {
            var userExist = await userManager.FindByEmailAsync(userSignupDTO.Email);

            if (userExist != null)
            {
                return new AuthServiceResponceDTO
                {
                    IsSucceed = false,
                    Message = "User already exists",
                };
            }

            var newUser = mapper.Map<AppIdentityUser>(userSignupDTO);

            if (await roleManager.RoleExistsAsync(role))
            {
                IdentityResult result = await userManager.CreateAsync(newUser, userSignupDTO.Password);

                if (!result.Succeeded)
                {
                    var errorString = new StringBuilder("User creating failed because: ");
                    foreach (var error in result.Errors)
                    {
                        errorString.AppendLine(error.Description);
                    }
                    return new AuthServiceResponceDTO
                    {
                        IsSucceed = false,
                        Message = errorString.ToString(),
                    };
                }
                
                var addedUser = await userManager.FindByEmailAsync(newUser.Email);
                var newDomainUser = mapper.Map<DomainUser>(addedUser);
                this.domainRepository.Create(newDomainUser);            

                await userManager.AddToRoleAsync(newUser, role);
                return new AuthServiceResponceDTO
                {
                    IsSucceed = true,
                    Message = "User created successfully."
                };              
            }
            else
            {
                return new AuthServiceResponceDTO
                {
                    IsSucceed = false,
                    Message = "This role doesn`t exist."
                };
            }
        }

        public async Task<AuthServiceResponceDTO> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            return new AuthServiceResponceDTO { IsSucceed = true, Message = "" };
        }

        public async Task<AuthServiceResponceDTO> MeAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                return new AuthServiceResponceDTO
                {
                    IsSucceed = true,
                    Message = userId.ToString(),
                };
            }

            return new AuthServiceResponceDTO()
            {
                IsSucceed = false,
                Message = "Unauthorized"
            };
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));

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
