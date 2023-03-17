using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class RemoveEntityCommand<TEntity> : IRequest
    where TEntity : Entity
{
    public RemoveEntityCommand(TEntity entity) => Entity = entity;
    public TEntity Entity { get; }
}

public class RemoveEntityCommandHandler<TEntity> : IRequestHandler<RemoveEntityCommand<TEntity>> 
    where TEntity : Entity
{
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public RemoveEntityCommandHandler(IUnitOfWork<ApplicationDbContext> context) 
        => _context = context;
    
    public Task<Unit> Handle(RemoveEntityCommand<TEntity> request, CancellationToken token)
    {
        var repository = _context.CreateRepository<TEntity, Guid>();

        repository.Remove(request.Entity);

        return Task.FromResult(Unit.Value);
    }
}

public class RemoveEntityCommandValidator : AbstractValidator<RemoveEntityCommand<Entity>>
{
    public RemoveEntityCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Id)
            .NotEqual(Guid.Empty);
    }
}