using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupMaterial;

public record GroupMaterialQuery(string Id, string? Fields) : IRequest<ExpandoObject>;
