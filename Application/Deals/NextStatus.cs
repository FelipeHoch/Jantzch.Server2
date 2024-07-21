using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.Deals;

public class NextStatus
{
    public record Command(string DealId) : IRequest<DealResponse>;

    public class Handler(
        IDealRepository dealRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Command, DealResponse>
    {
        public async Task<DealResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken) ?? throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });

            UserSimple user = new()
            {
                Id = jwtService.GetNameIdentifierFromToken(),
                Name = jwtService.GetNameFromToken()
            };

            try
            {
                deal.NextStatus(user);
            }
            catch (Exception)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, DealErrorMessages.INVALID_DEAL_STATUS);
            }           

            await dealRepository.UpdateAsync(deal, cancellationToken);

            return mapper.Map<DealResponse>(deal);
        }
    }
}
