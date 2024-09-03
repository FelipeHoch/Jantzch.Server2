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

public class EditDeal
{
    public class DealForEdit
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

    public record Command(string DealId, DealForEdit DealForEdit) : IRequest<DealResponse>;

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
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken);

            if (deal is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });
            }

            var soldedBy = await userRepository.GetByIdAsync(new ObjectId(request.DealForEdit.SoldedById), cancellationToken);

            if (soldedBy is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
            }

            var clientId = deal.Client.Id;

            deal.Phase = request.DealForEdit.Phase;
            deal.Type = request.DealForEdit.Type;
            deal.Description = request.DealForEdit.Description;
            deal.Value = request.DealForEdit.Value;
            deal.InstalationType = request.DealForEdit.InstalationType;
            deal.StructureType = request.DealForEdit.StructureType;
            deal.Status = request.DealForEdit.Status ?? StatusEnum.PendingMaterial;
            deal.ProjectStatus = request.DealForEdit.ProjectStatus ?? ProjectStatusEnum.GroupingPhotos;
            deal.Material = request.DealForEdit.Material;
            deal.InversorLocalization = request.DealForEdit.InversorLocalization;
            deal.DealConfirmedAt = request.DealForEdit.DealConfirmedAt;
            deal.SoldedBy = new UserSimple
            {
                Id = soldedBy.Id.ToString(),
                Name = soldedBy.Name,
            };
            deal.PaymentStatus = request.DealForEdit.PaymentStatus;
            deal.SystemPayment = request.DealForEdit.SystemPayment;
            deal.Commission = request.DealForEdit.Commission;
            deal.AppAccess = request.DealForEdit.AppAccess;
            deal.Datalogger = request.DealForEdit.Datalogger;
            deal.LinkForImages = request.DealForEdit.LinkForImages;
            deal.SolarEdge = request.DealForEdit.SolarEdge ?? false;

            if (clientId != request.DealForEdit.ClientId)
            {                
                var client = await clientsRepository.GetByIdAsync(new ObjectId(request.DealForEdit.ClientId), cancellationToken);

                if (client is null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
                }

                var localization = client.Localizations.Find(x => x.IsPrimary);

                if (localization is null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { message = ClientErrorMessages.ADDRESS_NOT_FOUND });
                }

                deal.CreatedBy = jwtService.GetNameFromToken();

                deal.Address = localization.Address;

                deal.Client = new ClientSimple
                {
                    Id = client.Id,
                    Name = client.Name,
                    Address = localization.Address,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber
                };
            }

            try
            {
                await dealRepository.UpdateAsync(deal, cancellationToken);
            }
            catch (Exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { message = DealErrorMessages.ERROR_CREATE });
            }

            return mapper.Map<DealResponse>(deal);
        }
    }
}
