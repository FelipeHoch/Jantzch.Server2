using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Deals;

public class ListDeals
{
    public record Query(DealsResourceParamenters Parameters) : IRequest<IEnumerable<ExpandoObject>>;

    public class Handler(
        IDealRepository dealRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    ) : IRequestHandler<Query, IEnumerable<ExpandoObject>>
    {
        public async Task<IEnumerable<ExpandoObject>> Handle(Query request, CancellationToken cancellationToken)
        {
            var deals = await dealRepository.GetAsync(request.Parameters, cancellationToken);

            paginationService.AddPaginationMetadataInResponseHeader(deals, httpContextAccessor.HttpContext.Response);

            var dealsList = deals.ToList();

            var dealsResponse = mapper.Map<List<DealResponse>>(dealsList);

            var dealsShaped = dataShapingService.ShapeDataList(dealsResponse, request.Parameters.Fields);

            return dealsShaped;
        }
    }
}
