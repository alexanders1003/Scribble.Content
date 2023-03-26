using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Web.Controllers.Base;
using Scribble.Responses;

namespace Scribble.Content.Web.Definitions.Documentation.Conventions;

public class UnitOfWorkControllerConversion : IApplicationModelConvention
{
    private static readonly Type[] ConventionTypes = {
        typeof(ApiResultResponse<ResponseEntityConventionStub>),
        typeof(ApiResultResponse<IEnumerable<ResponseEntityConventionStub>>),
        typeof(ApiResultResponse<IPagedCollection<ResponseEntityConventionStub>>)
    };
    
    public void Apply(ApplicationModel application)
    {
        var unitOfWorkControllers = application.Controllers.Where(x =>
            x.ControllerType.BaseType is not null && x.ControllerType.BaseType.IsGenericType &&
            (x.ControllerType.BaseType.GetGenericTypeDefinition() == typeof(UnitOfWorkReadOnlyController<,>) ||
             x.ControllerType.BaseType.GetGenericTypeDefinition() == typeof(UnitOfWorkWritableController<,,>)));
        
        foreach (var controllerModel in unitOfWorkControllers)
        {
            var responseType = controllerModel.ControllerType.BaseType?.GetGenericArguments().First();

            foreach (var actionModel in controllerModel.Actions) 
            {
                SetResponseUsingHack(actionModel, responseType);
            }
        }
    }

    private static void SetResponseUsingHack(ICommonModel actionModel, Type? responseType)
    {
        ArgumentNullException.ThrowIfNull(actionModel);
        ArgumentNullException.ThrowIfNull(responseType);

        var attributes = actionModel.Attributes;
        var attribute = FindResponseAttributeUsingHack(attributes);

        if (attribute is null) return;
        
        var type = ConventionTypes.First(x => x == attribute.Type);

        attribute.Type = BuildResponseType(type, responseType);
    }
    
    private static ProducesResponseTypeAttribute? FindResponseAttributeUsingHack(IEnumerable<object> attributes)
    {
        return attributes.OfType<ProducesResponseTypeAttribute>()
            .Where(x => ConventionTypes.Contains(x.Type))
            .FirstOrDefault(x => x.StatusCode is StatusCodes.Status200OK or StatusCodes.Status201Created);
    }

    private static Type BuildResponseType(Type type, Type responseType)
    {
        if (!type.IsGenericType)
            return responseType;

        var definition = type.GetGenericTypeDefinition();

        var constructedType = BuildResponseType(type.GetGenericArguments().First(), responseType);
        
        return definition.MakeGenericType(constructedType);
    }
}