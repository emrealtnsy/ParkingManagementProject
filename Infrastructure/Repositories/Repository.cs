using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Infrastructure.Data;

namespace ParkingManagement.Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _table = context.Set<T>();
    public IQueryable<T> GetQueryable() => _table;
    public async Task<T?> GetByIdAsync(Guid id) => await _table.FindAsync(id);
    public async Task AddAsync(T entity) => await _table.AddAsync(entity);
    public void Update(T entity) => _table.Update(entity);
    public void Delete(T entity) => _table.Remove(entity);


    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _table.AnyAsync(predicate);
}