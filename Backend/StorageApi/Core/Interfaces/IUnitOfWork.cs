
namespace StorageApi.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}