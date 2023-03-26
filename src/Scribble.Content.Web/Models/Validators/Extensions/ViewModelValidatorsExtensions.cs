using System.Reflection;
using FluentValidation;

namespace Scribble.Content.Web.Models.Validators.Extensions;

public static class ViewModelValidatorsExtensions
{
    public static void AddViewModelValidators(this IServiceCollection services)
    {
        var validators = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.BaseType is not null && x.BaseType.IsGenericType && x.BaseType
                .GetGenericTypeDefinition() == typeof(AbstractValidator<>));

        foreach (var validator in validators)
        {
            var type = validator.BaseType?
                .GetGenericArguments().FirstOrDefault();
            
            ArgumentNullException.ThrowIfNull(type);

            services.AddTransient(typeof(IValidator<>).MakeGenericType(type), validator);
        }
    }
}