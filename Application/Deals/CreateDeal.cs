 using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Deals;

public class CreateDeal
{
    public class DealForCreation
    {
        public string ClientId { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        public string InstalationType { get; set; }

        public string StructureType { get; set; }

        public string? PanelInstallation { get; set; }

        public string? Phase { get; set; }

        public StatusEnum? Status { get; set; } = StatusEnum.PendingMaterial;

        public ProjectStatusEnum? ProjectStatus { get; set; } = ProjectStatusEnum.GroupingPhotos;

        public string? Material { get; set; }

        public string? InversorLocalization { get; set; }

        public DateTime? DealConfirmedAt { get; set; }

        public string? SoldedById { get; set; }

        public PaymentStatusEnum? PaymentStatus { get; set; }

        public SystemPayment? SystemPayment { get; set; }

        public Commission? Commission { get; set; }

        public string? AppAccess { get; set; }

        public string? Datalogger { get; set; }

        public string? LinkForImages { get; set; }

        public bool? SolarEdge { get; set; }
    }

    public record Command(DealForCreation DealForCreation) : IRequest<DealResponse>;

    public class Handler(
        IDealRepository dealRepository,
        IClientsRepository clientsRepository,
        IUserRepository userRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Command, DealResponse>
    {
        public async Task<DealResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = await clientsRepository.GetByIdAsync(new ObjectId(request.DealForCreation.ClientId), cancellationToken);

            if (client is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
            }

            var address = client.Localizations.Find(x => x.IsPrimary);

            if (address is null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { message = ClientErrorMessages.ADDRESS_NOT_FOUND });
            }

            var soldedBy = await userRepository.GetByIdAsync(new ObjectId(request.DealForCreation.SoldedById), cancellationToken);

            if (soldedBy is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
            }

            var deal = mapper.Map<Deal>(request.DealForCreation);

            deal.CreatedBy = jwtService.GetNameFromToken();

            deal.Address = address.Address;

            deal.Client = new ClientSimple
            {
                Id = client.Id,
                Name = client.Name,
                Address = address.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };

            var user = new UserSimple
            {
                Id = jwtService.GetNameIdentifierFromToken(),
                Name = jwtService.GetNameFromToken()
            };

            var solderBy = new UserSimple
            {
                Id = soldedBy.Id.ToString(),
                Name = soldedBy.Name
            };

            deal.SoldedBy = solderBy;

            var historyStatus = new HistoryStatus
            {
                User = user,
                Status = deal.Status
            };

            deal.HistoryStatus.Add(historyStatus);

            try
            {
                await dealRepository.AddAsync(deal, cancellationToken);
            }
            catch (Exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { message = DealErrorMessages.ERROR_CREATE });
            }

            return mapper.Map<DealResponse>(deal);
        }
    }
}
