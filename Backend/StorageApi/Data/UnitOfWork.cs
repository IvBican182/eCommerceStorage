

using StorageApi.Data;

namespace StorageApi.Core.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Return a repository for any entity type
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        // Save changes synchronously
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        // Save changes asynchronously
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}