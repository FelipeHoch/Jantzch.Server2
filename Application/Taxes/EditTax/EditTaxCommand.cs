using Jantzch.Server2.Domain.Entities.Taxes;
using MediatR;

namespace Jantzch.Server2.Application.Taxes.EditTax;

public class EditTaxCommand
{
    public string Name { get; set; }

    public string Type { get; set; }

    public double Value { get; set; }

    public record Command(EditTaxCommand Model, string Id) : IRequest<Tax>;
}
