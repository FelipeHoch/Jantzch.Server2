using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Deals;

public class ListImages
{
    public record Query(string DealId) : IRequest<IEnumerable<Image>>;

    public class Handler(
        IDealRepository dealRepository
    ) : IRequestHandler<Query, IEnumerable<Image>>
    {
        public async Task<IEnumerable<Image>> Handle(Query request, CancellationToken cancellationToken)
        {
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken);

            if (deal is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });
            }

            return deal.GetImages();
        }
    }
}
