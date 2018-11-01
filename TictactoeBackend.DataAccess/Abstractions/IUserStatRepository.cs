using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TictactoeBackend.Entity.Concretes;

namespace TictactoeBackend.DataAccess.Abstractions
{
    public interface IUserStatRepository : IDataRepository<UserStat>
    {
        IList<UserStat> GetOrderedList(bool asc, params Expression<Func<UserStat, object>>[] navigationProperties);
    }
}
