using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2.Application.Clients.EditAddress;

public class EditAddressCommand
{
    public string Street { get; set; }

    public int StreetNumber { get; set; }

    public string District { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public Location PreviousLocation { get; set; }

    public record Command(EditAddressCommand Model, string Id) : IRequest<Client>;
}
