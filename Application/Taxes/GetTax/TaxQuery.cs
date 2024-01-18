using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Taxes.GetTax;

public record TaxQuery(string Id, string? Fields) : IRequest<ExpandoObject>;
