using FluentValidation;
using Scribble.Content.Web.Models.Entities;

namespace Scribble.Content.Web.Models.Validators;

public class LikeViewModelValidator : AbstractValidator<LikeViewModel>
{
    public LikeViewModelValidator()
    {
        RuleFor(x => x.PostId)
            .NotEqual(Guid.Empty);
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty);
    }
}