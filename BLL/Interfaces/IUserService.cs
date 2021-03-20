using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterRegularUser(UserDto userDto);
        Task<UserDto> RegisterAdministrator(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllRegularUsers();
        Task<IEnumerable<UserDto>> GetAllModerators();
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id, string token);
        Task<PublicUserInfoDto> GetPublicUserInfoById(int id);
        Task<bool> DeleteUser(int id, string token);
        Task<bool> UpdateUser(int id, UserDto user, string token);
        Task<bool> ChangePassword(int id, PasswordDto password, string token);
        Task PromoteUser(int id, string token);
        Task DemoteUser(int id, string token);
    }
}
