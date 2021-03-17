using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class File : EntityBase<int>
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }

    }
}
