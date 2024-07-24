using Domain.DTOs.Identity;
using Domain.DTOs.Services;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();

        Task<AdminServiceResponceDTO> DeleteUser(Guid userId);
    }
}
