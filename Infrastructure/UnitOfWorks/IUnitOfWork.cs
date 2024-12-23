using Microsoft.EntityFrameworkCore.Storage;
using ParkingManagement.Infrastructure.Repositories;

namespace ParkingManagement.Infrastructure.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    IDbContextTransaction BeginTransaction();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveAsync();
}