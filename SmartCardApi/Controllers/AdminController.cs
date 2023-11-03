using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.DTOs.Identity;

namespace SmartCardApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await this.adminService.GetUsers();
            return this.Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromBody] UserDeleteDTO dto)
        {
            var deletedUserResult = await this.adminService.DeleteUser(dto);

            if (deletedUserResult.IsSucceed)
            {          
                return this.Ok(deletedUserResult.Message);
            }

            return BadRequest();
        }
    }
}
