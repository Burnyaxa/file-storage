using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IShortLinkService
    {
        string GenerateShortLink(string link);
    }
}
