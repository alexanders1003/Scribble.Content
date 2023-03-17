using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;

namespace Scribble.Content.Infrastructure.UnitOfWork;

public interface IEntityRepository<TEntity, in TKey>
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    ICollection<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,        
        bool disableTracking = false,
        bool ignoreQueryFilters = false);
    
    Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,        
        bool disableTracking = false,
        bool ignoreQueryFilters = false,
        CancellationToken token = default);

    IPagedCollection<TEntity> GetPagedCollection(
        PaginationQueryParameters parameters,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false,
        bool ignoreQueryFilters = false);
    
    Task<IPagedCollection<TEntity>> GetPagedCollectionAsync(
        PaginationQueryParameters parameters,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false,
        CancellationToken token = default);

    TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false);
    
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false,
        CancellationToken token = default);

    TEntity? Find(params TKey[] keys);
    Task<TEntity?> FindAsync(params TKey[] keys);
    Task<TEntity?> FindAsync(TKey[] keys, CancellationToken token);

    TEntity Insert(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken token = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    int Count(Expression<Func<TEntity, bool>>? predicate = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default);

    bool Exists(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default);
}