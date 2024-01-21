using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2.Application.Clients.CreateClient;

public class CreateClientCommand : IRequest<Client>
{    
    public string Name { get; set; }

    public string? Email { get; set; }
 
    public string? Cnpj { get; set; }

    public string? Cpf { get; set; }

    public string PhoneNumber { get; set; }

    public Address Address { get; set; }
}
