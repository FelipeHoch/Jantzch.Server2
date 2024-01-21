using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using MediatR;

namespace Jantzch.Server2.Application.ReportConfigurations.CreateConfiguration;

public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand, ReportConfiguration>
{
    private readonly IReportConfigurationRepository _reportConfigurationRepository;

    public CreateConfigurationCommandHandler(IReportConfigurationRepository reportConfigurationRepository)
    {
        _reportConfigurationRepository = reportConfigurationRepository;
    }

    public async Task<ReportConfiguration> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var configuration = new ReportConfiguration
        {
            ReportKey = request.ReportKey,
            BottomTitle = request.BottomTitle,
            BottomText = request.BottomText,
            PhoneContact = request.PhoneContact,
            EmailContact = request.EmailContact,
            SiteUrl = request.SiteUrl,
        };

        await _reportConfigurationRepository.AddAsync(configuration, cancellationToken);

        await _reportConfigurationRepository.SaveChangesAsync(cancellationToken);

        return configuration;
    }
}
