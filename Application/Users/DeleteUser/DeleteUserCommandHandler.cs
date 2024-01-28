using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Users.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByIdpIdAsync(new ObjectId(request.Id), cancellationToken);

            if (user is null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
        }

        await _userRepository.DeleteAsync(user);

        await _userRepository.SaveChangesAsync(cancellationToken);
    }
}
