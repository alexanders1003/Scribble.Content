using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateEntityCommand<TEntity> : IRequest
    where TEntity : class
{
    public UpdateEntityCommand(TEntity entity) => Entity = entity;
    public TEntity Entity { get; }
}

public class UpdateEntityCommandHandler<TEntity> : IRequestHandler<UpdateEntityCommand<TEntity>> 
    where TEntity : class
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public UpdateEntityCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;

    public Task<Unit> Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.CreateRepository<TEntity, Guid>();

        repository.Update(request.Entity);

        return Task.FromResult(Unit.Value);
    }
}

public class UpdateBlogCommandValidator : AbstractValidator<UpdateEntityCommand<BlogEntity>>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Name)
            .NotNull().NotEmpty().MaximumLength(200);
        RuleFor(x => x.Entity.Description)
            .MaximumLength(1000);
        RuleFor(x => x.Entity.AuthorId)
            .NotEqual(Guid.Empty);
    }
}

public class UpdateArticleCommandValidator : AbstractValidator<UpdateEntityCommand<ArticleEntity>>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Title)
            .NotNull().NotEmpty().MaximumLength(500);
        RuleFor(x => x.Entity.Categories.Count)
            .GreaterThan(0);
    }
}

public class UpdateTagCommandValidator : AbstractValidator<UpdateEntityCommand<TagEntity>>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Name)
            .NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.Entity.AuthorId)
            .NotEqual(Guid.Empty);
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateEntityCommand<CategoryEntity>>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Name)
            .NotNull().NotEmpty();
    }
}

public class UpdateCommentCommandValidator : AbstractValidator<UpdateEntityCommand<CommentEntity>>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull();
        RuleFor(x => x.Entity.Text)
            .NotNull().NotEmpty();
        RuleFor(x => x.Entity.AuthorId)
            .NotEqual(Guid.Empty);
    }
}