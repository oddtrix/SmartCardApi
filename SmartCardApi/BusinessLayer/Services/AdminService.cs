using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.Models.DTOs.Identity;
using SmartCardApi.Models.DTOs.Services;
using SmartCardApi.Models.Identity;
using SmartCardApi.Models.User;

namespace SmartCardApi.BusinessLayer.Services
{
    public class AdminService : IAdminService
    {
        private UserManager<AppIdentityUser> userManager;

        private IAppDomainRepository repository;

        private IMapper mapper;

        public AdminService(UserManager<AppIdentityUser> userManager, IMapper mapper, IAppDomainRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<AdminServiceResponceDTO> DeleteUser(UserDeleteDTO dto)
        {
            var user = await userManager.FindByIdAsync(dto.Id.ToString());
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                this.repository.Delete(dto.Id);
                return new AdminServiceResponceDTO { IsSucceed = true, Message = $"{user.UserName} deleted" };
            }

            return new AdminServiceResponceDTO { IsSucceed = false, Message = "BadRequest" };
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var dtos = mapper.Map<IEnumerable<AppIdentityUser>, IEnumerable<UserDTO>>(users);
            return dtos;
        }
    }
}
