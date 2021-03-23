using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class PromotionException : BadRequestException
    {
        public override string Message { get; }

        public PromotionException(string errorMessage)
        {
            Message = errorMessage;
        }
    }
}
