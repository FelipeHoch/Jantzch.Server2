using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.GroupsMaterial.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.GroupsMaterial.DeleteGroupMaterial;

public class DeleteGroupMaterialCommandHandler
{
    public class Handler : IRequestHandler<DeleteGroupMaterialCommand>
    {
        private readonly IGroupsMaterialRepository _groupsMaterialRepository;

        public Handler(IGroupsMaterialRepository groupsMaterialRepository)
        {
            _groupsMaterialRepository = groupsMaterialRepository;
        }

        public async Task Handle(DeleteGroupMaterialCommand request, CancellationToken cancellationToken)
        {
            var groupMaterial = await _groupsMaterialRepository.GetGroupByIdAsync(ObjectId.Parse(request.Id), cancellationToken);

            if (groupMaterial is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = GroupMaterialErrorMessages.NOT_FOUND });
            }

            await _groupsMaterialRepository.DeleteGroupAsync(groupMaterial);

            await _groupsMaterialRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
