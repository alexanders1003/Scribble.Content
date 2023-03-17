using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetEntityPagedQuery<TEntity> : IRequest<IPagedCollection<TEntity>>
    where TEntity : Entity
{
    public GetEntityPagedQuery(PaginationQueryParameters parameters) 
        => Parameters = parameters;
    public PaginationQueryParameters Parameters { get; }
}

public class GetEntityPagedQueryHandler<TEntity> : IRequestHandler<GetEntityPagedQuery<TEntity>, IPagedCollection<TEntity>> 
    where TEntity : Entity
{
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetEntityPagedQueryHandler(IUnitOfWork<ApplicationDbContext> context) 
        => _context = context;
    
    public async Task<IPagedCollection<TEntity>> Handle(GetEntityPagedQuery<TEntity> request, CancellationToken token)
    {
        var repository = _context.CreateRepository<TEntity, Guid>();
        
        return await repository.GetPagedCollectionAsync(request.Parameters, token: token)
            .ConfigureAwait(false);
    }
}