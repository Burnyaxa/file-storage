using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class EmailAlreadyTakenException : BadRequestException
    {
        public override string Message { get; }

        public EmailAlreadyTakenException(string email)
        {
            Message = $"Email '{email}' is already taken";
        }

    }
}
