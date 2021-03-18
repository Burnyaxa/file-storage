using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Exceptions
{
    public class BadPasswordException : BadRequestException
    {
        public override string Message { get; }

        public BadPasswordException(IEnumerable<IdentityError> errors) : base()
        {
            Message = errors.Aggregate("", (msg, next) => msg + $"{next.Description} ");
        }
    }
}
