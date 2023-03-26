using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class PostViewModelValidator : AbstractValidator<PostViewModel>
{
    public PostViewModelValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty().MaximumLength(500);
        RuleFor(x => x.Content)
            .NotNull().NotEmpty();
        RuleFor(x => x.BlogId)
            .NotEqual(Guid.Empty);
    }
}