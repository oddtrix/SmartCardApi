using AutoMapper;
using Domain.DTOs.Identity;
using Domain.DTOs.Services;
using Domain.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartCardApi.BusinessLayer.Interfaces;

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

        public async Task<AdminServiceResponceDTO> DeleteUser(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                this.repository.Delete(userId);
                return new AdminServiceResponceDTO { IsSucceed = true, Message = $"{user.UserName} deleted" };
            }

            return new AdminServiceResponceDTO { IsSucceed = false, Message = "BadRequest" };
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var dtos = mapper.Map<IEnumerable<AppIdentityUser>, IEnumerable<UserDTO>>(users);
            return dtos;
        }
    }
}
