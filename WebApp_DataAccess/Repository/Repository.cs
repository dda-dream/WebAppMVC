using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext _db)
        {
            db = _db;
            dbSet = db.Set<T>();
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if(filter!= null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if(!isTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if(filter!= null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (orderBy != null)
                query = orderBy(query);

            if(!isTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public IEnumerable<LogTableModel> GetLogForId(int? id)
        {
            string tableName = typeof(T).Name;

            var retVal = db.LogTable.Where(l => l.LogRecordId == id)
                                    .Where(q => q.LogTableName == tableName)
                                    .OrderBy(l => l.CreatedDateTime)
                                    .ToList();
            return retVal;
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }


        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges(); 
        }

        public void RemoveRange(IEnumerable<T> entitys)
        {
            dbSet.RemoveRange(entitys);
        }
    }
}
