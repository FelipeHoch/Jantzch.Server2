using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Enums;
using MediatR;

namespace Jantzch.Server2.Application.Clients.CreateClient;

public class CreateClientCommand : IRequest<Client>
{    
    public string Name { get; set; }

    public string? Email { get; set; }

    public string? Cnpj { get; set; }

    public string? Cpf { get; set; }

    public string PhoneNumber { get; set; }

    public List<LocalizationToCreate> Localizations { get; set; } = [];

    public List<ClientType> Types { get; set; } = [];
}

public class LocalizationToCreate
{
    public Address? Address { get; set; }

    public string? Description { get; set; }

    public bool IsPrimary { get; set; }
}