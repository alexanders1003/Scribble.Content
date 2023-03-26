using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class TagViewModelValidator : AbstractValidator<TagViewModel>
{
    public TagViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().MaximumLength(100);
    }
}