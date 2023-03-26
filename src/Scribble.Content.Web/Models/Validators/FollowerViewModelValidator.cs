using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class FollowerViewModelValidator : AbstractValidator<FollowerViewModel>
{
    public FollowerViewModelValidator()
    {
        RuleFor(x => x.FollowerId)
            .NotEqual(Guid.Empty);
        RuleFor(x => x.BlogId)
            .NotEqual(Guid.Empty);
    }
}