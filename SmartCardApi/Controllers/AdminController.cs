using Domain.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCardApi.BusinessLayer.Interfaces;

namespace SmartCardApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [Route("users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Users()
        {
            var users = await this.adminService.GetAllUsers();
            return this.Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var deletedUserResult = await this.adminService.DeleteUser(userId);

            if (deletedUserResult.IsSucceed)
            {          
                return this.Ok(deletedUserResult.Message);
            }

            return BadRequest();
        }
    }
}
