using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Repositories
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(Guid id);
        IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();        
    }
}
