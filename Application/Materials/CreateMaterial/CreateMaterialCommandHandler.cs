﻿using Jantzch.Server2.Application.Materials.Notifications;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Materials.CreateMaterial;

public class CreateMaterialCommandHandler
{
    public class Handler : IRequestHandler<CreateMaterialCommand, Material>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IGroupsMaterialRepository _groupsMaterialRepository;

        private readonly IMediator _mediator;

        public Handler(
            IMaterialsRepository materialsRepository, 
            IGroupsMaterialRepository groupsMaterialRepository,
            IMediator mediator)
        {
            _materialsRepository = materialsRepository;

            _groupsMaterialRepository = groupsMaterialRepository;

            _mediator = mediator;
        }

        public async Task<Material> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = new Material
            {
                Name = request.Name,
                Value = request.Value,
                Eu = request.Eu,
                CreatedBy = request.CreatedBy,
                GroupIdObject = ObjectId.Parse(request.GroupId)
            };            

            var group = await _groupsMaterialRepository.GetGroupByIdAsync(ObjectId.Parse(request.GroupId), cancellationToken);                

            if (group is null)
                throw new RestException(HttpStatusCode.NotFound, new { Group = "Not found" });


            await _materialsRepository.AddMaterialAsync(material);

            await _materialsRepository.SaveChangesAsync();

            await _mediator.Publish(new MaterialCreatedNotification(material.Id.Value.ToString(), material.Name), cancellationToken);

            return material;
        }
    }
}
