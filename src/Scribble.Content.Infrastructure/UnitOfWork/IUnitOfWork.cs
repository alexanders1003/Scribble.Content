using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Scribble.Content.Infrastructure.UnitOfWork.Factories;

namespace Scribble.Content.Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    TDbContext DbContext { get; }
}

public interface IUnitOfWork : IEntityRepositoryFactory, IDisposable
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken token = default);

    IDbContextTransaction BeginTransaction(bool reuse = false);
    Task<IDbContextTransaction> BeginTransactionAsync(bool reuse = false, CancellationToken token = default);
}