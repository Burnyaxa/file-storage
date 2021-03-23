using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class NotEnoughRightsException : ForbiddenException
    {
        public override string Message { get; }

        public NotEnoughRightsException()
        {
            Message = "Not enough rights do perform this operation";
        }
    }
}
