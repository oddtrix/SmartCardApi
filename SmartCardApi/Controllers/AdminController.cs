using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.DTOs.Identity;
using SmartCardApi.Models.Identity;
using SmartCardApi.Models.User;

namespace SmartCardApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private UserManager<AppIdentityUser> userManager;

        private IAppDomainRepository repository;

        private IMapper mapper;

        public AdminController(UserManager<AppIdentityUser> userManager, IMapper mapper, IAppDomainRepository repository)
        {
            this.userManager = userManager;   
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();
            List<UserDTO> usersCutInfo = new List<UserDTO>();
            foreach (var user in users)
            {
                usersCutInfo.Add(mapper.Map<UserDTO>(user));
            }
            return this.Ok(usersCutInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromBody] UserDeleteDTO dto)
        {
            var user = await userManager.FindByIdAsync(dto.Id.ToString());
            var result = await userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                this.repository.Delete(dto.Id);
                return this.Ok(user.UserName + " deleted");
            }
            return BadRequest();
        }
    }
}
