using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.Deals;

public class DeleteDeal
{
    public record Command(string DealId) : IRequest;

    public class Handler(
        IDealRepository dealRepository
    ) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken) ?? throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });

            await dealRepository.DeleteAsync(deal.Id, cancellationToken);                       
        }
    }
}