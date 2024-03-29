﻿using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.GroupsMaterial.Constants;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.Materials.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Features.Materials.EditMaterial;

public class EditMaterialCommandHandler
{
    public class Handler : IRequestHandler<EditMaterialCommand.Command, Material>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IGroupsMaterialRepository _groupsMaterialRepository;

        public Handler(IMaterialsRepository materialsRepository, IGroupsMaterialRepository groupsMaterialRepository)
        {
            _materialsRepository = materialsRepository;
            _groupsMaterialRepository = groupsMaterialRepository;
        }

        public async Task<Material> Handle(EditMaterialCommand.Command request, CancellationToken cancellationToken)

        {
            var material = await _materialsRepository.GetMaterialByIdAsync(ObjectId.Parse(request.Id));

            if (material is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = MaterialErrorMessages.NOT_FOUND });
            }

            if (material.GroupMaterialId is not null)
            {
                var group = await _groupsMaterialRepository.GetGroupByIdAsync((ObjectId)material.GroupMaterialId, cancellationToken);

                if (group is null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { message = GroupMaterialErrorMessages.NOT_FOUND });
                }
            }


            material.Name = request.Model.Name;
            material.Value = request.Model.Value;
            material.Eu = request.Model.Eu;
            material.GroupMaterialId = ObjectId.Parse(request.Model.GroupId);

            await _materialsRepository.UpdateMaterialAsync(material);

            await _materialsRepository.SaveChangesAsync();

            return material;
        }
    }
}
