using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.CreateGroupMaterial;

public class CreateGroupMaterialCommandHandler
{
    public class Handler : IRequestHandler<CreateGroupMaterialCommand, GroupMaterial>
    {
        private readonly IGroupsMaterialRepository _groupMaterialRepository;

        public Handler(IGroupsMaterialRepository groupMaterialRepository)
        {
            _groupMaterialRepository = groupMaterialRepository;
        }

        public async Task<GroupMaterial> Handle(CreateGroupMaterialCommand request, CancellationToken cancellationToken)
        {
            var groupMaterial = new GroupMaterial
            {
                Name = request.Name,
                Description = request.Description,
            };

            await _groupMaterialRepository.AddGroupAsync(groupMaterial, cancellationToken);

            await _groupMaterialRepository.SaveChangesAsync(cancellationToken);

            return groupMaterial;
        }
    }
}
