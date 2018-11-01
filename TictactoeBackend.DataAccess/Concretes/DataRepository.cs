using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using TictactoeBackend.Data;
using TictactoeBackend.DataAccess.Abstractions;
using TictactoeBackend.Entity.Abstractions;

namespace TictactoeBackend.DataAccess.Concretes
{
    public class DataRepository<T> : IDataRepository<T> where T : class, IEntity
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new TictactoeDBContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }

            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new TictactoeDBContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }

            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = new TictactoeDBContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }

            return item;
        }

        public virtual void Add(params T[] items)
        {
            Update(items);
        }

        public virtual void Update(params T[] items)
        {
            using (var context = new TictactoeDBContext())
            {
                DbSet<T> dbSet = context.Set<T>();
                foreach (T item in items)
                {
                    dbSet.Add(item);
                    foreach (DbEntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
                    {
                        IEntity entity = entry.Entity;
                        entry.State = GetEntityState(entity.EntityState);
                    }
                }
                context.SaveChanges();
            }
        }

        public virtual void Remove(params T[] items)
        {
            Update(items);
        }

        // EntityStater this method switches entitystates to make real methods above
        protected static System.Data.Entity.EntityState GetEntityState(TictactoeBackend.Entity.Abstractions.EntityState entityState)
        {
            switch (entityState)
            {
                case TictactoeBackend.Entity.Abstractions.EntityState.Unchanged:
                    return System.Data.Entity.EntityState.Unchanged;
                case TictactoeBackend.Entity.Abstractions.EntityState.Added:
                    return System.Data.Entity.EntityState.Added;
                case TictactoeBackend.Entity.Abstractions.EntityState.Modified:
                    return System.Data.Entity.EntityState.Modified;
                case TictactoeBackend.Entity.Abstractions.EntityState.Deleted:
                    return System.Data.Entity.EntityState.Deleted;
                default:
                    return System.Data.Entity.EntityState.Detached;
            }
        }
    }
}
