using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IJwtFactory
    {
        JwtSecurityToken DecodeToken(string token);
        string GenerateEncodedToken(string userName, ClaimsIdentity identity);
        Task<ClaimsIdentity> GenerateClaimsIdentityAsync(User user);
        int GetUserIdClaim(string token);
        string GetUserRoleClaim(string token);
    }
}
