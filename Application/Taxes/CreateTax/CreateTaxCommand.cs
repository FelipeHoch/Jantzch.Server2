using Jantzch.Server2.Domain.Entities.Taxes;
using MediatR;

namespace Jantzch.Server2.Application.Taxes.CreateTax;

public class CreateTaxCommand : IRequest<Tax>
{
    public string Name { get; set; }

    public string Type { get; set; }

    public double Value { get; set; }

}
