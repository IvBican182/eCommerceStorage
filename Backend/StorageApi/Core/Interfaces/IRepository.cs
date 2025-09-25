
using System.Linq.Expressions;

namespace StorageApi.Core.Interfaces
{
    public interface IRepository<T> where T : class
    { 
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}