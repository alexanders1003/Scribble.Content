using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class BlogViewModelValidator : AbstractValidator<BlogViewModel>
{
    public BlogViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description)
            .MaximumLength(1000);
        RuleFor(x => x.AuthorId)
            .NotEqual(Guid.Empty);
    }
}