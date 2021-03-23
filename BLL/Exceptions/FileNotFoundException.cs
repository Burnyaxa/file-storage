using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class FileNotFoundException : NotFoundException
    {
        public override string Message { get; }

        public FileNotFoundException(string link)
        {
            Message = $"File with short link {link} not found";
        }
    }
}
