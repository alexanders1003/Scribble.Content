using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
{
    public CommentViewModelValidator()
    {
        RuleFor(x => x.Text)
            .NotNull().NotEmpty();
        RuleFor(x => x.PostId)
            .NotEqual(Guid.Empty);
        RuleFor(x => x.AuthorId)
            .NotEqual(Guid.Empty);
    }
}