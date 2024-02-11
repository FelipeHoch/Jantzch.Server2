using Jantzch.Server2.Application.Materials.DeleteMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.Materials.Constants;
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
            if (request.Id == MaterialsConstants.OthersMaterialId)
                throw new RestException(HttpStatusCode.BadRequest, new { message = MaterialErrorMessages.CANT_DELETE });

            var material = await _materialsRepository.GetMaterialByIdAsync(ObjectId.Parse(request.Id));

            if (material is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = MaterialErrorMessages.NOT_FOUND });
            }

            await _materialsRepository.DeleteMaterialAsync(material);

            await _materialsRepository.SaveChangesAsync();
        }
    }
}
