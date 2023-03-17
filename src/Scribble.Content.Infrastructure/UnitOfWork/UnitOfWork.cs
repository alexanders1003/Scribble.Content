using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Scribble.Content.Infrastructure.UnitOfWork;

public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> 
    where TDbContext : DbContext
{
    private bool _disposed;
    private Dictionary<Type, object>? _repositories;

    public UnitOfWork(TDbContext context)
        => DbContext = context ?? throw new ArgumentNullException(nameof(context));

    public TDbContext DbContext { get; }

    public IEntityRepository<TEntity, TKey> CreateRepository<TEntity, TKey>()
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        _repositories ??= new Dictionary<Type, object>();
        
        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
            _repositories[type] = new EntityRepository<TEntity, TKey>(DbContext);

        return (IEntityRepository<TEntity, TKey>)_repositories[type];
    }

    public int SaveChanges() => DbContext.SaveChanges();

    public async Task<int> SaveChangesAsync(CancellationToken token = default) => await DbContext.SaveChangesAsync(token);

    public IDbContextTransaction BeginTransaction(bool reuse = false)
    {
        var transaction = DbContext.Database.CurrentTransaction;
        if (transaction is null)
            return DbContext.Database.BeginTransaction();

        return reuse ? transaction : DbContext.Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(bool reuse = false, CancellationToken token = default)
    {
        var transaction = DbContext.Database.CurrentTransaction;
        if (transaction is null)
            return await DbContext.Database.BeginTransactionAsync(token);

        return reuse ? transaction : await DbContext.Database.BeginTransactionAsync(token);
    }

    public void Dispose()
    {
        Dispose(true);
        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();
                DbContext.Dispose();
            }
        }

        _disposed = true;
    }
}