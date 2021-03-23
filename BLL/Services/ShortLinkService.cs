using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using NeoSmart.Utils;

namespace BLL.Services
{
    public class ShortLinkService : IShortLinkService
    {
        public string GenerateShortLink(string link)
        {
            var bytes = Encoding.UTF8.GetBytes(link);
            return UrlBase64.Encode(bytes);
        }
    }
}
