using Jantzch.Server2.Application.Clients.GetClients;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Clients.GetClientsInformation;

public class GetClientsInformationHandler : IRequestHandler<ClientsInformationQuery, IEnumerable<ExpandoObject>>
{
    private readonly IClientsRepository _clientsRepository;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetClientsInformationHandler(
        IClientsRepository clientsRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IDataShapingService dataShapingService
    )
    {
        _clientsRepository = clientsRepository;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(ClientsInformationQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientsRepository.GetInformationsAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(clients, _httpContextAccessor.HttpContext.Response);

        var clientsList = clients.ToList();

        var clientsShaped = _dataShapingService.ShapeDataList(clientsList, request.Parameters.Fields);

        return clientsShaped;
    }
}
