using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
