using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterRegularUserAsync(UserDto userDto);
        Task<UserDto> RegisterAdministratorAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllRegularUsersAsync();
        Task<IEnumerable<UserDto>> GetAllModeratorsAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id, string token);
        Task<PublicUserInfoDto> GetPublicUserInfoByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id, string token);
        Task<bool> UpdateUserAsync(int id, UserDto user, string token);
        Task<bool> ChangePasswordAsync(int id, PasswordDto password, string token);
        Task PromoteUserAsync(int id, string token);
        Task DemoteUserAsync(int id, string token);
    }
}
