 using Jantzch.Server2.Application.Shared;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.ReportConfigurations.GetReportConfigurations;

public record ConfigurationsQuery(ResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
