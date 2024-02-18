using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Events.GetEvents;

public record EventsQuery(EventResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
