using AutoMapper;
using Jantzch.Server2.Application.Users.Models;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Security;
using MediatR;
using MongoDB.Bson;
using System.Text.Json;

namespace Jantzch.Server2.Application.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userFromIdpDto = Utils.DecodeBase64<UserFromIdpDto>(request.Data);

        if (userFromIdpDto is null)
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = UserErrorMessages.INVALID_USER_DATA });
        }

        userFromIdpDto.Token = null;

        var user = _mapper.Map<User>(userFromIdpDto, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.CustByHour = request.UserFromClient.CustByHour;
                dest.Types = request.UserFromClient.Types;
                dest.Id = ObjectId.GenerateNewId();
            }));

        user.Email = user.Email.ToLower();

        await _userRepository.AddAsync(user, cancellationToken);
        
        await _userRepository.SaveChangesAsync(cancellationToken);

        var userResponse = _mapper.Map<UserResponse>(user);

        if (userFromIdpDto.NewPassword is not null) userResponse.NewPassword = userFromIdpDto.NewPassword;

        return userResponse;
    }
}
