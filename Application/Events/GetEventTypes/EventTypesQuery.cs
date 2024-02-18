using Jantzch.Server2.Application.Shared;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Events.GetEventTypes;

public record EventTypesQuery(ResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
