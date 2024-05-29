using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        T? Get(Expression<Func<T,bool>> filter);

        IEnumerable<T> GetAll();

        void Remove(int id);
    }
}
