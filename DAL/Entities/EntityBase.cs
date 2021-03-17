using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;

namespace DAL.Entities
{
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : struct
    {
        public abstract TKey Id { get; set; }
    }
}
