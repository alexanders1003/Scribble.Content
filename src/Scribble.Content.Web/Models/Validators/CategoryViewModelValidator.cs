using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    public CategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().MaximumLength(100);
    }
}