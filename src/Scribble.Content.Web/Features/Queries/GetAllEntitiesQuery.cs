using System.Linq.Expressions;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetAllEntitiesQuery<TEntity, TKey> : IRequest<IEnumerable<TEntity>>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    public GetAllEntitiesQuery(Expression<Func<TEntity, bool>>? predicate = default) => Predicate = predicate;
    public Expression<Func<TEntity, bool>>? Predicate { get; }
}

public class GetAllEntitiesQueryHandler<TEntity, TKey> : IRequestHandler<GetAllEntitiesQuery<TEntity, TKey>, IEnumerable<TEntity>>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    private readonly ILogger<GetAllEntitiesQueryHandler<TEntity, TKey>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    
    public GetAllEntitiesQueryHandler(ILogger<GetAllEntitiesQueryHandler<TEntity, TKey>> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TEntity>> Handle(GetAllEntitiesQuery<TEntity, TKey> request, CancellationToken token)
    {
        var repository = _unitOfWork.GetRepository<TEntity, Guid>();

        return await repository.GetAllAsync(request.Predicate, token: token)
            .ConfigureAwait(false);
    }
}