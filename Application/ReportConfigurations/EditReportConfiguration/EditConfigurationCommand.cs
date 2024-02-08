using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using MediatR;

namespace Jantzch.Server2.Application.ReportConfigurations.EditConfiguration;

public class EditConfigurationCommand
{
    public string BottomTitle { get; set; }

    public string BottomText { get; set; }

    public string PhoneContact { get; set; }

    public string EmailContact { get; set; }

    public string SiteUrl { get; set; }

    public double MinValue { get; set; }

    public record Command(EditConfigurationCommand Model, string Key) : IRequest<ReportConfiguration>;
}
