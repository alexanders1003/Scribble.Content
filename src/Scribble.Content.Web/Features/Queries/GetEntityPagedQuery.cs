using System.Linq.Expressions;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetEntityPagedQuery<TEntity, TKey> : IRequest<IPagedCollection<TEntity>>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    public GetEntityPagedQuery(PaginationQueryParameters parameters, Expression<Func<TEntity, bool>>? predicate = null)
    {
        Parameters = parameters;
        Predicate = predicate;
    }
    
    public PaginationQueryParameters Parameters { get; }
    public Expression<Func<TEntity, bool>>? Predicate { get; }
}

public class GetEntityPagedQueryHandler<TEntity, TKey> : IRequestHandler<GetEntityPagedQuery<TEntity, TKey>, IPagedCollection<TEntity>> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    private readonly ILogger<GetEntityPagedQueryHandler<TEntity, TKey>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetEntityPagedQueryHandler(ILogger<GetEntityPagedQueryHandler<TEntity, TKey>> logger, 
        IUnitOfWork<ApplicationDbContext> context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IPagedCollection<TEntity>> Handle(GetEntityPagedQuery<TEntity, TKey> request, CancellationToken token)
    {
        var repository = _context.GetRepository<TEntity, Guid>();
        
        return await repository.GetPagedCollectionAsync(request.Parameters, request.Predicate, token: token)
            .ConfigureAwait(false);
    }
}