using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class EntityNotFoundException : NotFoundException
    {
        public override string Message { get; }

        public EntityNotFoundException(string type, int id)
        {
            Message = $"Couldn't find {type} with id {id}";
        }
    }
}
