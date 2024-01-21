using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.ReportConfigurations.EditConfiguration;

public class EditReportConfigurationCommandHandler
{
    public class Handler : IRequestHandler<EditConfigurationCommand.Command, ReportConfiguration>
    {
        private readonly IReportConfigurationRepository _reportConfigurationRepository;

        public Handler(IReportConfigurationRepository reportConfRepository)
        {
            _reportConfigurationRepository = reportConfRepository;
        }

        public async Task<ReportConfiguration> Handle(EditConfigurationCommand.Command request, CancellationToken cancellationToken)
        {
            var configuration = await _reportConfigurationRepository.GetByKeyAsync(request.Key, cancellationToken);

            if (configuration is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ReportConfiguration = Constants.NOT_FOUND });
            }

            configuration.ReportKey = request.Model.ReportKey;
            configuration.BottomTitle = request.Model.BottomTitle;
            configuration.BottomText = request.Model.BottomText;
            configuration.PhoneContact = request.Model.PhoneContact;
            configuration.EmailContact = request.Model.EmailContact;
            configuration.SiteUrl = request.Model.SiteUrl;

            await _reportConfigurationRepository.UpdateAsync(configuration);

            await _reportConfigurationRepository.SaveChangesAsync(cancellationToken);

            return configuration;
        }
    }
}
