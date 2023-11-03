using SmartCardApi.Models.DTOs.Identity;
using SmartCardApi.Models.DTOs.Services;

namespace SmartCardApi.BusinessLayer.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserDTO>> GetUsers();

        Task<AdminServiceResponceDTO> DeleteUser(UserDeleteDTO dto);
    }
}
