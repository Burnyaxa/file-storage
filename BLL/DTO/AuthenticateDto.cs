using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class AuthenticateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
