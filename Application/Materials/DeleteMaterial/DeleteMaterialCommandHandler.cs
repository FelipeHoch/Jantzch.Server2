using Jantzch.Server2.Application.Materials.DeleteMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Features.Materials;

public class DeleteMaterialCommandHandler
{
    public class Handler : IRequestHandler<DeleteMaterialCommand.Command>
    {
        private readonly IMaterialsRepository _materialsRepository;

        public Handler(IMaterialsRepository materialsRepository)
        {
            _materialsRepository = materialsRepository;
        }

        public async Task Handle(DeleteMaterialCommand.Command request, CancellationToken cancellationToken)
        {
            var material = await _materialsRepository.GetMaterialById(ObjectId.Parse(request.Id));

            if (material == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Material = "Not found" });
            }

            await _materialsRepository.DeleteMaterial(material);

            await _materialsRepository.SaveChangesAsync();

            await Task.FromResult(Unit.Value);
        }
    }
}
