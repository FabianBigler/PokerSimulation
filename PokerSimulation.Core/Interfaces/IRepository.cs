using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PokerSimulation.Core.Repositories
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
