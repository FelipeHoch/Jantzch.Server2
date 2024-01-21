using MediatR;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;

namespace Jantzch.Server2.Application.ReportConfigurations.CreateConfiguration;

public class CreateConfigurationCommand : IRequest<ReportConfiguration>
{
    public string ReportKey { get; set; }

    public string BottomTitle { get; set; }

    public string BottomText { get; set; }

    public string PhoneContact { get; set; }

    public string EmailContact { get; set; }

    public string SiteUrl { get; set; }
}
