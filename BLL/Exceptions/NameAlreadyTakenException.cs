using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class NameAlreadyTakenException : BadRequestException
    {
        public override string Message { get; }

        public NameAlreadyTakenException(string name)
        {
            Message = $"Name '{name}' is already taken";
        }
    }
}
