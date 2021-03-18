using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class File : EntityBase<int>
    {
        public string Name { get; set; }
        public bool IsShared { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime Uploaded { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StatisticsId { get; set; }
        public FileStatistics Statistics { get; set; }
    }
}
