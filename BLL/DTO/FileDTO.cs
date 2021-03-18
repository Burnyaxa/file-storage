using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsShared { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime Uploaded { get; set; }
        public DateTime LastUpdated { get; set; }
        
        public int CreatorId { get; set; }
        public string CreatorUserName { get; set; }

        public int Downloads { get; set; }
        public int Views { get; set; }
    }
}
