using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Jantzch.Server2.Application.Users.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand.Command, User>
{
    private readonly IUserRepository _userRepository;

    public EditUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(EditUserCommand.Command request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByIdpIdAsync(new ObjectId(request.Id), cancellationToken);

            if (user is null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
        }
        
        request.Model.ApplyTo(user);

        user.Email = user.Email.ToLower();

        await _userRepository.UpdateAsync(user);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }
}
