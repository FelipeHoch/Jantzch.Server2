using MediatR;

namespace Jantzch.Server2.Application.Taxes.DeleteTax;

public record DeleteTaxCommand(string Id) : IRequest;
