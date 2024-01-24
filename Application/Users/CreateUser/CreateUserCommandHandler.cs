using AutoMapper;
using Jantzch.Server2.Application.Users.Models;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Security;
using MediatR;
using MongoDB.Bson.IO;
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
        //var userFromIdpDto = JsonSerializer.Deserialize<UserFromIdpDto>(Utils.DecodeBase64<string>(request.Data)!);

        //if (userFromIdpDto is null)
        //{
        //    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { User = "Invalid user data" });
        //}

        //userFromIdpDto.Token = null;

        //var user = _mapper.Map<User>(userFromIdpDto, opt =>
        //    opt.AfterMap((src, dest) =>
        //    {
        //        dest.CustByHour = 
        //    });

        //await _userRepository.CreateUserAsync(user);

        //return _mapper.Map<UserResponse>(user);
    }
}
