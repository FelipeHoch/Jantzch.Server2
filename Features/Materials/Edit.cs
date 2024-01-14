using FluentValidation;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infraestructure.Repositories.GroupsMaterial;
using Jantzch.Server2.Infraestructure.Repositories.Materials;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Features.Materials;

public class Edit
{
    public class MaterialData
    {
        public string Name { get; set; }
     
        public double Value { get; set; }

        public string Eu { get; set; }

        public string? GroupId { get; set; }
    }

    public record Command(Model Model, string id) : IRequest<MaterialEnvelope>;

    public record Model(MaterialData Material);

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Material).NotNull();
        }
    }

    public class Handler : IRequestHandler<Command, MaterialEnvelope>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IGroupsMaterialRepository _groupsMaterialRepository;

        public Handler(IMaterialsRepository materialsRepository, IGroupsMaterialRepository groupsMaterialRepository)
        {
            _materialsRepository = materialsRepository;
            _groupsMaterialRepository = groupsMaterialRepository;
        }

        public async Task<MaterialEnvelope> Handle(Command request, CancellationToken cancellationToken)
        {
            var material = await _materialsRepository.GetMaterialById(ObjectId.Parse(request.id));

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


            material.Name = request.Model.Material.Name;
            material.Value = request.Model.Material.Value;
            material.Eu = request.Model.Material.Eu;
            material.GroupIdObject = ObjectId.Parse(request.Model.Material.GroupId);

            await _materialsRepository.UpdateMaterial(material);

            await _materialsRepository.SaveChangesAsync();

            return new MaterialEnvelope(material);
        }
    }
}
