using MediatR;

namespace Jantzch.Server2.Application.ReportConfigurations.DeleteReportConfiguration;

public record DeleteConfigurationCommand(string Id) : IRequest;