using System;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryPattern
{
    public interface IRepositoryBase<T>
    {

        T Find(int id);

        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}
