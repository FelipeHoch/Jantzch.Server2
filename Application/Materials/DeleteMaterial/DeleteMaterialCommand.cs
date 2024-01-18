using MediatR;

namespace Jantzch.Server2.Application.Materials.DeleteMaterial;

public class DeleteMaterialCommand
{
    public record Command(string Id) : IRequest;
}
