namespace Analysis.Domain.RepositoryInterfaces;

public interface IGenericRepository
{
    public interface IGenericRepository<T>
    {
        Task<IList<T>> GetAllAsync();
        Task<T?> GetByReferenceAsync(string reference);
        IQueryable<T> GetAllQueryable();
    }
}
