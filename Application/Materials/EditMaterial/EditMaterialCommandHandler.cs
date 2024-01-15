﻿using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
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
            var material = await _materialsRepository.GetMaterialById(ObjectId.Parse(request.Id));

            if (material == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Material = "Not found" });
            }

            if (material.GroupIdObject is not null)
            {
                var group = await _groupsMaterialRepository.GetGroupById((ObjectId)material.GroupIdObject);

                if (group == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Group = "Not found" });
                }
            }


            material.Name = request.Model.Name;
            material.Value = request.Model.Value;
            material.Eu = request.Model.Eu;
            material.GroupIdObject = ObjectId.Parse(request.Model.GroupId);

            await _materialsRepository.UpdateMaterial(material);

            await _materialsRepository.SaveChangesAsync();

            return material;
        }
    }
}