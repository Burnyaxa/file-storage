using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _manager;
        private readonly IJwtFactory _factory;

        public AuthService(UserManager<User> manager, IJwtFactory factory)
        {
            _manager = manager;
            _factory = factory;
        }
        public async Task<AuthenticateDto> AuthenticateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var identity = await GetClaimsIdentityAsync(userDto);
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var token = _factory.GenerateEncodedToken(userDto.UserName, identity);
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return new AuthenticateDto()
            {
                Id = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier).Value),
                UserName = identity.FindFirst(ClaimTypes.Name).Value,
                Role = identity.FindFirst(ClaimTypes.Role).Value,
                Token = token
            };
        }
        private async Task<ClaimsIdentity> GetClaimsIdentityAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.UserName == null)
            {
                throw new ArgumentNullException(nameof(user.UserName));
            }

            if (user.Password == null)
            {
                throw new ArgumentNullException(nameof(user.Password));
            }

            var userToVerify = await _manager.FindByNameAsync(user.UserName);
            if (userToVerify == null)
            {
                throw new CredentialsException();
            }

            if (await _manager.CheckPasswordAsync(userToVerify, user.Password))
            {
                return await _factory.GenerateClaimsIdentityAsync(userToVerify);
            }

            throw new CredentialsException();
        }
    }
}
