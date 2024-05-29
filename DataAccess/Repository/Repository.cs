using DataAccess.Repository.IRepository;
using FindDoctor.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _db { get; set; }

        public DbSet<T> dbSet { get; set; }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet=_db.Set<T>();
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public T? Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;

            query=query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
           
            return query.ToList();
        }

        public void Remove(int id)
        {
            T entity=dbSet.Find(id);

            dbSet.Remove(entity);
        }
    }
}
