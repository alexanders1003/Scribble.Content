using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetArticlePagedByTagIdQuery : IRequest<IPagedCollection<ArticleEntity>>
{
    public GetArticlePagedByTagIdQuery(Guid tagId, PaginationQueryParameters parameters)
    {
        TagId = tagId;
        Parameters = parameters;
    }

    public Guid TagId { get; }
    public PaginationQueryParameters Parameters { get; }
}

public class GetArticlePagedByTagIdQueryHandler : IRequestHandler<GetArticlePagedByTagIdQuery, IPagedCollection<ArticleEntity>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetArticlePagedByTagIdQueryHandler(IUnitOfWork<ApplicationDbContext> context) 
        => _context = context;

    public async Task<IPagedCollection<ArticleEntity>> Handle(GetArticlePagedByTagIdQuery request, CancellationToken token)
    {
        var repository = _context.CreateRepository<ArticleEntity, Guid>();
        
        return await repository.GetPagedCollectionAsync(request.Parameters, x => x.Tags.Any(tag => tag.Id == request.TagId), 
                token: token)
            .ConfigureAwait(false);
    }
}

public class GetArticlePagedByTagIdQueryValidator : AbstractValidator<GetArticlePagedByTagIdQuery>
{
    public GetArticlePagedByTagIdQueryValidator()
    {
        RuleFor(x => x.TagId)
            .NotEqual(Guid.Empty);
        RuleFor(x => x.Parameters.PageIndex)
            .GreaterThan(0);
        RuleFor(x => x.Parameters.PageSize)
            .GreaterThan(0);
        RuleFor(x => x.Parameters.IndexFrom)
            .GreaterThanOrEqualTo(0);
    }
}