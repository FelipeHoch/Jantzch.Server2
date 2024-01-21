using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.ReportConfigurations.GetReportConfiguration;

public record ConfigurationQuery(string Key, string? Fields) : IRequest<ExpandoObject>;