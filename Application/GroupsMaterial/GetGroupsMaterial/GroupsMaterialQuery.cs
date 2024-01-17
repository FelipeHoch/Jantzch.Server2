using Jantzch.Server2.Application.Shared;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupsMaterial;

public record GroupsMaterialQuery(ResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
