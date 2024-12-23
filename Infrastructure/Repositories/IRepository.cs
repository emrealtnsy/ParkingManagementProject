using System.Linq.Expressions;

namespace ParkingManagement.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task<T?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}