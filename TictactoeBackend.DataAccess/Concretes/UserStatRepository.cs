using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TictactoeBackend.Data;
using TictactoeBackend.DataAccess.Abstractions;
using TictactoeBackend.Entity.Concretes;

namespace TictactoeBackend.DataAccess.Concretes
{
    public class UserStatRepository: DataRepository<UserStat>, IUserStatRepository
    {
        public virtual IList<UserStat> GetOrderedList(bool asc, params Expression<Func<UserStat, object>>[] navigationProperties)
        {
            List<UserStat> list;
            using (var context = new TictactoeDBContext())
            {
                IQueryable<UserStat> dbQuery = context.Set<UserStat>();

                //Apply eager loading
                foreach (Expression<Func<UserStat, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<UserStat, object>(navigationProperty);

                if (asc)
                {
                    list = dbQuery
                        .AsNoTracking()
                        .Where(x=>!x.User.Username.Contains("Guest"))
                        .OrderBy(x=>x.Point)
                        .ThenBy(x=>x.WinStreak)
                        .ThenBy(x=>x.TotalWins)
                        .ThenByDescending(x=>x.TotalLoses)
                        .ToList<UserStat>();
                }
                else
                {
                    list = dbQuery
                        .AsNoTracking()
                        .Where(x => !x.User.Username.Contains("Guest"))
                        .OrderByDescending(x => x.Point)
                        .ThenByDescending(x => x.WinStreak)
                        .ThenByDescending(x => x.TotalWins)
                        .ThenBy(x => x.TotalLoses)
                        .ToList<UserStat>();
                }

            }

            return list;
        }
    }
}
