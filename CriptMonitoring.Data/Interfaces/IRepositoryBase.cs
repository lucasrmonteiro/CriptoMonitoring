namespace CriptMonitoring.Data.Interfaces;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T?>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<T?> InsertAsync(T? entity);
    Task UpdateAsync(string id, T? entity);
    Task DeleteAsync(string id);
}