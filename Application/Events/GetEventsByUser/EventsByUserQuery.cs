using System.Dynamic;
using Jantzch.Server2.Application.Events;
using MediatR;

namespace Jantzch.Server2;

public record EventsByUserQuery(EventResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;