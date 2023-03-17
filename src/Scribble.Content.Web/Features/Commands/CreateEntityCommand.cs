using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateEntityCommand<TEntity> : IRequest<TEntity>
    where TEntity : class
{
    public CreateEntityCommand(TEntity model) => Model = model;
    public TEntity Model { get; }
}

public class CreateEntityCommandHandler<TEntity> : IRequestHandler<CreateEntityCommand<TEntity>, TEntity> 
    where TEntity : class
{
    private readonly ILogger<CreateEntityCommandHandler<TEntity>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public CreateEntityCommandHandler(ILogger<CreateEntityCommandHandler<TEntity>> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> Handle(CreateEntityCommand<TEntity> request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<TEntity, Guid>();

        return await repository.InsertAsync(request.Model, token)
            .ConfigureAwait(false);
    }
}

public class CreateBlogCommandValidator : AbstractValidator<CreateEntityCommand<BlogEntity>>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
        RuleFor(x => x.Model.Name)
            .NotNull().NotEmpty().MaximumLength(200);
        RuleFor(x => x.Model.Description)
            .MaximumLength(1000);
        RuleFor(x => x.Model.AuthorId)
            .NotEqual(Guid.Empty);
    }
}

public class CreateArticleCommandValidator : AbstractValidator<CreateEntityCommand<ArticleEntity>>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
        RuleFor(x => x.Model.Title)
            .NotNull().NotEmpty().MaximumLength(500);
        RuleFor(x => x.Model.Categories.Count)
            .GreaterThan(0);
    }
}

public class CreateTagCommandValidator : AbstractValidator<CreateEntityCommand<TagEntity>>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
        RuleFor(x => x.Model.Name)
            .NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model.AuthorId)
            .NotEqual(Guid.Empty);
    }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateEntityCommand<CategoryEntity>>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
        RuleFor(x => x.Model.Name)
            .NotNull().NotEmpty();
    }
}

public class CreateCommentCommandValidator : AbstractValidator<CreateEntityCommand<CommentEntity>>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
        RuleFor(x => x.Model.Text)
            .NotNull().NotEmpty();
        RuleFor(x => x.Model.AuthorId)
            .NotEqual(Guid.Empty);
    }
}