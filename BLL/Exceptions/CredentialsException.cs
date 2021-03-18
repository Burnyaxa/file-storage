using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class CredentialsException : BadRequestException
    {
        public override string Message { get; }

        public CredentialsException() : base()
        {
            Message = "Wrong credentials were entered";
        }
    }
}
