using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCardApi.Models.DTOs.Identity;
using SmartCardApi.Models.Identity;

namespace SmartCardApi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private UserManager<AppIdentityUser> userManager;

        private IMapper mapper;

        public AdminController(UserManager<AppIdentityUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;   
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();
            List<UserDTO> usersCutInfo = new List<UserDTO>();
            foreach (var user in users)
            {
                usersCutInfo.Add(mapper.Map<UserDTO>(user));
            }
            return usersCutInfo;
        }
    }
}
