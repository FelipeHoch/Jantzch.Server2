using FluentValidation;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Orders;
using Microsoft.AspNetCore.JsonPatch;

namespace Jantzch.Server2.Application.Orders.EditOrder;

public class EditOrderCommandValidator : AbstractValidator<EditOrderCommand.Command>
{
    private readonly IPropertyCheckerService _propertyCheckerService;

    public EditOrderCommandValidator(IPropertyCheckerService propertyCheckerService)
    {
        _propertyCheckerService = propertyCheckerService;

        RuleFor(x => x.Model).NotNull().Must(IsValidProperties);
        RuleFor(x => x.Id).NotNull();
    }

    private bool IsValidProperties(JsonPatchDocument<Order> doc)
    {
        var path = doc.Operations.Select(x => x.path.Replace("/", "")).ToList();

        var fields = string.Join(",", path);

        return _propertyCheckerService.TypeHasProperties<Order>(fields);
    }
}
