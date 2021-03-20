using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateDto> AuthenticateAsync(UserDto userDto);
    }
}
