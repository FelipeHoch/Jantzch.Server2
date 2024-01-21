using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Clients.GetClients;

public class GetClientsHandler : IRequestHandler<ClientsQuery, IEnumerable<ExpandoObject>>
{
    private readonly IClientsRepository _clientsRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetClientsHandler(
        IClientsRepository clientsRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _clientsRepository = clientsRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(ClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientsRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(clients, _httpContextAccessor.HttpContext.Response);

        var clientsList = clients.ToList();

        var clientsResponse = _mapper.Map<List<ClientResponse>>(clientsList);

        var clientsShaped = _dataShapingService.ShapeDataList(clientsResponse, request.Parameters.Fields);

        return clientsShaped;
    }
}
