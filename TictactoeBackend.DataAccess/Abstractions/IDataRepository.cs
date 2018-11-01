using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TictactoeBackend.Entity.Abstractions;

namespace TictactoeBackend.DataAccess.Abstractions
{
    public interface IDataRepository<T> where T : class, IEntity 
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);
    }
}
