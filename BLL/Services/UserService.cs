using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _manager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> manager, IJwtFactory jwtFactory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _manager = manager;
            _jwtFactory = jwtFactory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterRegularUserAsync(UserDto userDto)
        {
            return await RegisterToRoleAsync("RegularUser", userDto);
        }

        public async Task<UserDto> RegisterAdministratorAsync(UserDto userDto)
        {
            return await RegisterToRoleAsync("Administrator", userDto);
        }

        public async Task<IEnumerable<UserDto>> GetAllRegularUsersAsync()
        {
            return await GetUsersByRoleAsync("RegularUser");
        }

        public async Task<IEnumerable<UserDto>> GetAllModeratorsAsync()
        {
            return await GetUsersByRoleAsync("Administrator");
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            var entities = await _manager.Users.ToListAsync();
            foreach (var user in entities)
            {
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = (await _manager.GetRolesAsync(user)).First();
                users.Add(userDto);
            }

            return users;
        }

        public async Task<UserDto> GetUserByIdAsync(int id, string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var user = await _manager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), id);
            }

            var requesterId = _jwtFactory.GetUserIdClaim(token);
            if (requesterId == id)
            {
                return _mapper.Map<User, UserDto>(user);
            }

            var requesterRoleClaim = _jwtFactory.GetUserRoleClaim(token);

            switch (requesterRoleClaim)
            {
                case "Admin":
                    return _mapper.Map<User, UserDto>(user);
                case "Moderator":
                {
                    var roles = await _manager.GetRolesAsync(user);
                    if (roles.Any(r => r == "Moderator" || r == "Admin"))
                    {
                        throw new NotEnoughRightsException();
                    }
                    return _mapper.Map<User, UserDto>(user);
                }
                default:
                    throw new NotEnoughRightsException();
            }
        }

        public async Task<PublicUserInfoDto> GetPublicUserInfoByIdAsync(int id)
        {
            var user = await _manager.FindByIdAsync(id.ToString());
            if (user == null) throw new EntityNotFoundException(nameof(user), id);

            return _mapper.Map<PublicUserInfoDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id, string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var user = await _manager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), id);
            }

            var requesterId = _jwtFactory.GetUserIdClaim(token);
            if (requesterId == id)
            {
                return (await _manager.DeleteAsync(user)).Succeeded;
            }

            var requesterRoleClaim = _jwtFactory.GetUserRoleClaim(token);
            if (requesterRoleClaim == "Admin")
            {
                return (await _manager.DeleteAsync(user)).Succeeded;
            }
            throw new NotEnoughRightsException();
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto user, string token)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var requesterId = _jwtFactory.GetUserIdClaim(token);
            var requesterRole = _jwtFactory.GetUserRoleClaim(token);

            var userEntity = await _manager.FindByIdAsync(id.ToString());
            if (userEntity == null)
            {
                throw new EntityNotFoundException(nameof(userEntity), id);
            }

            if (requesterId != id && requesterRole != "Admin")
            {
                throw new NotEnoughRightsException();
            }

            if (user.UserName != null && !userEntity.UserName.Equals(user.UserName))
            {
                var isNameTaken = await _manager.FindByNameAsync(user.UserName);
                if (isNameTaken != null)
                {
                    throw new NameAlreadyTakenException(user.UserName);
                }
                userEntity.UserName = user.UserName;
            }

            if (user.Email != null && !userEntity.Email.Equals(user.Email))
            {
                var isEmailTaken = await _manager.FindByEmailAsync(user.Email);
                if (isEmailTaken != null)
                {
                    throw new EmailAlreadyTakenException(user.Email);
                }
                userEntity.Email = user.Email;
            }

            return (await _manager.UpdateAsync(userEntity)).Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(int id, PasswordDto password, string token)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (password.NewPassword == null)
            {
                throw new ArgumentNullException(nameof(password.NewPassword));
            }

            if (password.OldPassword == null)
            {
                throw new ArgumentNullException(nameof(password.OldPassword));
            }

            var requesterId = _jwtFactory.GetUserIdClaim(token);
            var requesterRole = _jwtFactory.GetUserRoleClaim(token);
            var userEntity = await _manager.FindByIdAsync(id.ToString());
            if (userEntity == null)
            {
                throw new EntityNotFoundException(nameof(userEntity), id);
            }

            if (requesterId != id && requesterRole != "Admin")
            {
                throw new NotEnoughRightsException();
            }

            bool checkPassword = await _manager.CheckPasswordAsync(userEntity, password.OldPassword);
            if (checkPassword == false)
            {
                throw new CredentialsException();
            }

            return (await _manager.ChangePasswordAsync(userEntity, password.OldPassword, password.NewPassword)).Succeeded;
        }

        public async Task PromoteUserAsync(int id, string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var user = await _manager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), id);
            }

            var requesterRoleClaim = _jwtFactory.GetUserRoleClaim(token);
            if (requesterRoleClaim != "Admin")
            {
                throw new NotEnoughRightsException();
            }

            var result = await _manager.RemoveFromRoleAsync(user, "RegularUser");
            if (!result.Succeeded)
            {
                throw new PromotionException("Couldn't delete user from Regular role");
            }

            result = await _manager.AddToRoleAsync(user, "Moderator");
            if (!result.Succeeded)
            {
                throw new PromotionException("Couldn't add user to Moderator role");
            }
        }

        public async Task DemoteUserAsync(int id, string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var user = await _manager.FindByIdAsync(id.ToString());
            if (user == null) throw new EntityNotFoundException(nameof(user), id);

            var requesterRoleClaim = _jwtFactory.GetUserRoleClaim(token);
            if (requesterRoleClaim != "Admin") throw new NotEnoughRightsException();

            var result = await _manager.RemoveFromRoleAsync(user, "Moderator");
            if (!result.Succeeded) throw new PromotionException("Couldn't delete user from Moderator role");

            result = await _manager.AddToRoleAsync(user, "RegularUser");
            if (!result.Succeeded) throw new PromotionException("Couldn't add user to Regular role");
        }

        private async Task<User> CreateUserAsync(UserDto userDto)
        {
            if (await _manager.FindByEmailAsync(userDto.Email) != null)
            {
                throw new EmailAlreadyTakenException(userDto.Email);
            }

            if (await _manager.FindByNameAsync(userDto.UserName) != null)
            {
                throw new NameAlreadyTakenException(userDto.UserName);
            }

            var user = _mapper.Map<UserDto, User>(userDto);
            var result = await _manager.CreateAsync(user, userDto.Password);

            if (result.Errors.Any())
            {
                throw new BadPasswordException(result.Errors);
            }

            return result.Succeeded ? await _manager.FindByNameAsync(user.UserName) : null;
        }

        private async Task<UserDto> RegisterToRoleAsync(string role, UserDto userDto)
        {
            var user = await CreateUserAsync(userDto);

            if (user == null)
            {
                throw new ArgumentNullException($"{nameof(user)}", "Couldn't create a user");
            }

            await _manager.AddToRoleAsync(user, role);
            return _mapper.Map<User, UserDto>(user);
        }

        private async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
           return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(
                (await _manager.GetUsersInRoleAsync(role)).ToList());
        }
    }
}
