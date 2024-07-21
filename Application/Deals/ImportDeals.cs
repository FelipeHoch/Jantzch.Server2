using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Excel;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Clients.Enums;
using Jantzch.Server2.Domain.Entities.Clients.Services;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using MongoDB.Bson;
using System.Collections.Concurrent;

namespace Jantzch.Server2.Application.Deals;

public class ImportDeals
{
    public record Command(IFormFile File) : IRequest<IEnumerable<DealResponse>>;

    public class Handler(
        IExcelService excelService,
        IClientsRepository clientsRepository,
        IMapper mapper,
        IClientService clientService,
        IJwtService jwtService,
        IDealRepository dealRepository
    ) : IRequestHandler<Command, IEnumerable<DealResponse>>
    {
        public async Task<IEnumerable<DealResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            using var stream = request.File.OpenReadStream();

            var deals = excelService.ReadExcel(stream);

            var emails = deals.Select(deal => deal.Email).Where(email => !string.IsNullOrWhiteSpace(email)).Distinct().ToList();
            var names = deals.Select(deal => deal.ClientName).Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().ToList();
            var phones = deals.Select(deal => deal.PhoneNumber).Where(phone => !string.IsNullOrWhiteSpace(phone)).Distinct().ToList();

            var integrationsIds = deals.Select(deal => deal.IntegrationId).Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList();

            var dealsFromDb = await dealRepository.GetByIntegrationIdsAsync(integrationsIds, cancellationToken);

            var clients = await clientsRepository.GetByMultipleIdentifiersAsync(emails, phones, names, cancellationToken);

            var tasks = new List<Task>();
            var newClients = new ConcurrentBag<Client>();

            foreach (var deal in deals)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var client = clients.FirstOrDefault(c => (c.Email == deal.Email && !string.IsNullOrWhiteSpace(deal.Email)) || (c.PhoneNumber == deal.PhoneNumber && !string.IsNullOrWhiteSpace(deal.PhoneNumber)) || c.Name.ToLower() == deal.ClientName.ToLower());

                    if (client is null)
                    {
                        client = new Client
                        {
                            Email = deal.Email,
                            Name = deal.ClientName,
                            PhoneNumber = deal.PhoneNumber,
                            Types = [ClientType.Solar]
                        };

                        var localization = await clientService.GetLocalization(deal.Address, true, deal.InstalationType);

                        if (localization is not null)
                        {
                            client.Localizations.Add(localization);
                        }
                       
                        newClients.Add(client);
                    }
                }, cancellationToken));
            }

            await Task.WhenAll(tasks);
            
            if (newClients.Any())
            {
                var newUniqueClients = newClients.GroupBy(c => c.Name).Select(c => c.First());

                await clientsRepository.AddAsync(newUniqueClients, cancellationToken);
                clients.AddRange(newClients);
            }

            List<Deal> dealsToCreate = [];

            foreach (var deal in deals)
            {
                var client = clients.FirstOrDefault(c => (c.Email == deal.Email && !string.IsNullOrWhiteSpace(deal.Email)) || (c.PhoneNumber == deal.PhoneNumber && !string.IsNullOrWhiteSpace(deal.PhoneNumber)) || c.Name.ToLower() == deal.ClientName.ToLower());                

                var dealFromDb = dealsFromDb.FirstOrDefault(d => d.IntegrationId == deal.IntegrationId);

                if (dealFromDb is not null)
                {
                    continue;
                }

                var user = new UserSimple
                {
                    Id = jwtService.GetNameIdentifierFromToken(),
                    Name = jwtService.GetNameFromToken()
                };

                var historyStatus = new HistoryStatus
                {
                    User = user
                };

                var newDeal = new Deal
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Value = deal.Value,
                    CreatedBy = deal.CreatedBy,
                    Phase = deal.Phase,
                    CreatedAt = deal.CreatedAt,
                    DealConfirmedAt = deal.DealConfirmedAt,
                    StructureType = deal.StructureType,
                    Description = deal.Description,
                    Type = deal.Type,
                    Address = deal.Address,
                    InstalationType = deal.InstalationType,
                    Client = new ClientSimple
                    {
                        Id = client.Id,
                        Name = client.Name
                    },
                    IntegrationId = deal.IntegrationId,
                    Status = StatusEnum.PendingMaterial,
                    LastUpdateAt = DateTime.UtcNow,              
                    HistoryStatus = [historyStatus]
                };

                dealsToCreate.Add(newDeal);
            }

            if (dealsToCreate.Any())
            {
                await dealRepository.AddAsync(dealsToCreate, cancellationToken);
            }


            return mapper.Map<IEnumerable<DealResponse>>(dealsToCreate);
        }
    }
}
