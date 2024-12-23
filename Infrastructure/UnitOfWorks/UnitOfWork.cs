using Microsoft.EntityFrameworkCore.Storage;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;

namespace ParkingManagement.Infrastructure.UnitOfWorks;

public class UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider) : IUnitOfWork
{
    private bool _disposed;
    public IRepository<T> Repository<T>() where T : class
        => serviceProvider.GetRequiredService<IRepository<T>>();

    public IDbContextTransaction BeginTransaction() => context.Database.BeginTransaction();

    public async Task CommitTransactionAsync() => await context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync() => await context.Database.RollbackTransactionAsync();
    public async Task SaveAsync() => await context.SaveChangesAsync();

    public void Dispose()
    {
        if (!_disposed)
        {
            context.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}