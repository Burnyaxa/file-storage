using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public IEnumerable<File> Files { get; set; }
    }
}
