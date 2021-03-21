using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class EntityCollectionNotFoundException : NotFoundException
    {
        public override string Message { get; }

        public EntityCollectionNotFoundException(string type)
        {
            Message = $"Entity collection with type {type} not found";
        }
    }
}
