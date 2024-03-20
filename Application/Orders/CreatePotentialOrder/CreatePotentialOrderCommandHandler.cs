using System.Net;
using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2;

public class CreatePotentialOrderCommandHandler : IRequestHandler<CreatePotentialOrderCommand, PotentialOrder>
{
    private readonly IPotentialOrderRepository _potentialOrderRepository;

    private readonly IClientsRepository _clientsRepository;

    private readonly IJwtService _jwtService;

    private readonly IMapper _mapper;

    public CreatePotentialOrderCommandHandler(
        IPotentialOrderRepository potentialOrderRepository,
        IClientsRepository clientsRepository,
        IMapper mapper, 
        IJwtService jwtService
    )
    {
        _potentialOrderRepository = potentialOrderRepository;

        _clientsRepository = clientsRepository;

        _mapper = mapper;

        _jwtService = jwtService;
    }

    public async Task<PotentialOrder> Handle(CreatePotentialOrderCommand request, CancellationToken cancellationToken)
    {
        var potentialOrder = _mapper.Map<PotentialOrder>(request);

        var client = await _clientsRepository.GetByIdAsync(new ObjectId(request.Client.Id), cancellationToken);

        if (client is null) {
            throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
        }

        var user = new UserSimple
        {
            Id = _jwtService.GetNameIdentifierFromToken(),
            Name = _jwtService.GetNameFromToken()
        };

        potentialOrder.RelateUser(user);

        await _potentialOrderRepository.AddAsync(potentialOrder, cancellationToken);

        return potentialOrder;
    }
}
