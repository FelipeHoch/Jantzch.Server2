using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Taxes.GetTaxes;

public record TaxesQuery(TaxesResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
