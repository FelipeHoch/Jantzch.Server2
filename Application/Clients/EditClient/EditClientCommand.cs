using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2.Application.Clients.EditClient;

public class EditClientCommand
{
    public string Name { get; set; }

    public string? Email { get; set; }

    public string? Cnpj { get; set; }

    public string? Cpf { get; set; }
   
    public string PhoneNumber { get; set; }

    public record Command(EditClientCommand Model, string Id) : IRequest<Client>;
}
