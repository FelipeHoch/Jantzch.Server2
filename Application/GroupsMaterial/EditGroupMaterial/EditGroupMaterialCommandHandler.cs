using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.GroupsMaterial.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.GroupsMaterial.EditGroupMaterial;

public class EditGroupMaterialCommandHandler
{
    public class Handler : IRequestHandler<EditGroupMaterialCommand.Command, GroupMaterial>
    {
        private readonly IGroupsMaterialRepository _groupMaterialRepository;

        public Handler(IGroupsMaterialRepository groupMaterialRepository)
        {
            _groupMaterialRepository = groupMaterialRepository;
        }

        public async Task<GroupMaterial> Handle(EditGroupMaterialCommand.Command request, CancellationToken cancellationToken)
        {
            var groupMaterial = await _groupMaterialRepository.GetGroupByIdAsync(new ObjectId(request.Id), cancellationToken);

            if (groupMaterial is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = GroupMaterialErrorMessages.NOT_FOUND });
            }

            groupMaterial.Name = request.Model.Name;
            groupMaterial.Description = request.Model.Description;

            await _groupMaterialRepository.UpdateGroupAsync(groupMaterial);

            await _groupMaterialRepository.SaveChangesAsync(cancellationToken);

            return groupMaterial;
        }
    }
}
