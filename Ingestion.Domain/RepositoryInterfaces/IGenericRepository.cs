namespace Ingestion.Domain.RepositoryInterfaces;

public interface IGenericRepository
{
    public interface IGenericRepository<T>
    {
        Task<IList<T>> GetAllAsync();
        Task<T?> GetByReferenceAsync(string reference);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string reference);
    }
}
