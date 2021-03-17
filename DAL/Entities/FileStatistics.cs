using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class FileStatistics : EntityBase<int>
    {
        public int Views { get; set; }
        public int Downloads { get; set; }

        public File File { get; set; }
    }
}
