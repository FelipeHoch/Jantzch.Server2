using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.ReportConfigurations.DeleteReportConfiguration;

public class DeleteConfigurationCommandHandler : IRequestHandler<DeleteConfigurationCommand>
{
    private readonly IReportConfigurationRepository _repository;

    public DeleteConfigurationCommandHandler(IReportConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteConfigurationCommand request, CancellationToken cancellationToken)
    {
        var configuration = await _repository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (configuration is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { ReportConfiguration = "Not found" });
        }

        await _repository.DeleteAsync(configuration);

        await _repository.SaveChangesAsync(cancellationToken);

        await Task.FromResult(Unit.Value);
    }
}
