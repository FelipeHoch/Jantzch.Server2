using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Configuration;
using Jantzch.Server2.Application.Users.Models;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Security;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Auth.AuthUserFromIdp;

public class AuthUserFromIdpHandler : IRequestHandler<AuthUserFromIdpQuery, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfigurationService _configurationService;

    public AuthUserFromIdpHandler(
        IUserRepository userRepository, 
        IMapper mapper,
        IConfigurationService configurationService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configurationService = configurationService;
    }

    public async Task<string> Handle(AuthUserFromIdpQuery request, CancellationToken cancellationToken)
    {
        var userFromIdp = Utils.DecodeBase64<UserFromIdpDto>(request.DataInBase64);

        if (userFromIdp is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = "Usuário não encontrado" });
        }

        var user = await _userRepository.GetByIdpIdAsync(new ObjectId(userFromIdp.Id), cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(userFromIdp.Email, cancellationToken);

            if (user is null)
                throw new RestException(HttpStatusCode.NotFound, new { message = "Usuário não encontrado" });

            user.IdentityProviderId = new ObjectId(userFromIdp.Id);

            await _userRepository.UpdateAsync(user);

            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        var response = _mapper.Map<AuthResponse>(user);

        response.Token = userFromIdp.Token;

        var host = _configurationService.GetRedirectAuth() + Utils.ObjectToBase64(response);

        return host;
    }
}
