using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2.Application.Clients.EditAddress;

public class EditAddressCommand
{
    public string Id { get; set; }

    public string Street { get; set; }

    public int StreetNumber { get; set; }

    public string District { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Description { get; set; }    

    public bool? IsPrimary { get; set; }

    public record Command(EditAddressCommand Model, string Id) : IRequest<Client>;
}
