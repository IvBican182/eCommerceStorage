

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StorageApi.Core.Interfaces;
using StorageApi.Data;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual IQueryable<T> GetAll()
    {
        return _dbSet; 
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual void Add(T entity) => _dbSet.Add(entity);
    public virtual void Update(T entity) => _dbSet.Update(entity);
    public virtual void Remove(T entity) => _dbSet.Remove(entity);

    public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }
}