using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Scribble.Content.Infrastructure.UnitOfWork.Extensions;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;

namespace Scribble.Content.Infrastructure.UnitOfWork;

public class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> 
    where TEntity : class 
    where TKey : IEquatable<TKey>
{
    private readonly DbSet<TEntity> _entities;

    public EntityRepository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _entities = context.Set<TEntity>();
    }

    public ICollection<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();
        

        return orderBy is not null
            ? orderBy(query).ToCollection()
            : query.ToCollection();
    }

    public async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false, 
        CancellationToken token = default)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();
        

        return orderBy is not null
            ? await orderBy(query).ToCollectionAsync()
            : await query.ToCollectionAsync();
    }

    public IPagedCollection<TEntity> GetPagedCollection(
        PaginationQueryParameters parameters,
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return orderBy is not null
            ? orderBy(query).ToPagedCollection(parameters)
            : query.ToPagedCollection(parameters);
    }

    public async Task<IPagedCollection<TEntity>> GetPagedCollectionAsync(
        PaginationQueryParameters parameters,
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false, 
        CancellationToken token = default)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return orderBy is not null
            ? await orderBy(query).ToPagedCollectionAsync(parameters, token)
            : await query.ToPagedCollectionAsync(parameters, token);
    }

    public TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return orderBy is not null
            ? orderBy(query).FirstOrDefault()
            : query.FirstOrDefault();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false, 
        bool ignoreQueryFilters = false,
        CancellationToken token = default)
    {
        var query = _entities.AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return orderBy is not null
            ? orderBy(query).FirstOrDefaultAsync(token)
            : query.FirstOrDefaultAsync(token);
    }

    public TEntity? Find(params TKey[] keys)
    {
        return _entities.Find(keys);
    }

    public async Task<TEntity?> FindAsync(params TKey[] keys)
    {
        return await _entities.FindAsync(keys)
            .ConfigureAwait(false);
    }

    public async Task<TEntity?> FindAsync(TKey[] keys, CancellationToken token)
    {
        return await _entities.FindAsync(new object?[] { keys, token }, cancellationToken: token)
            .ConfigureAwait(false);
    }

    public TEntity Insert(TEntity entity) => _entities.Add(entity).Entity;

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken token = default)
    {
        var entryEntity = await _entities.AddAsync(entity, token)
            .ConfigureAwait(false);

        return entryEntity.Entity;
    }

    public void Update(TEntity entity) => _entities.Update(entity);

    public void Remove(TEntity entity) => _entities.Remove(entity);

    public int Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate is not null
            ? _entities.Count(predicate)
            : _entities.Count();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default)
    {
        return predicate is not null
            ? await _entities.CountAsync(predicate, token)
            : await _entities.CountAsync(token);
    }

    public bool Exists(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate is not null
            ? _entities.Any(predicate)
            : _entities.Any();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken token = default)
    {
        return predicate is not null
            ? await _entities.AnyAsync(predicate, token)
            : await _entities.AnyAsync(token);
    }
}