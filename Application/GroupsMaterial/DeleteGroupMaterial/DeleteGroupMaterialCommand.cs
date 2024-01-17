using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.DeleteGroupMaterial;

public record DeleteGroupMaterialCommand(string Id) : IRequest;
