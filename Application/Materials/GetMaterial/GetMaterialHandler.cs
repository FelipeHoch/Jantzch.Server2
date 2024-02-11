using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Domain.Entities.Materials.Constants;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.Materials.GetMaterial;

public class GetMaterialHandler
{
    public class Handler : IRequestHandler<MaterialQuery, ExpandoObject>
    {
        private readonly IMaterialsRepository _materialRepository;

        private readonly IMapper _mapper;

        private readonly IDataShapingService _dataShapingService;

        public Handler(
            IMaterialsRepository materialRepository,
            IMapper mapper,
            IDataShapingService dataShapingService
        )
        {
            _materialRepository = materialRepository;

            _mapper = mapper;

            _dataShapingService = dataShapingService;
        }

        public async Task<ExpandoObject> Handle(MaterialQuery request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(new ObjectId(request.Id));

            if (material is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = MaterialErrorMessages.NOT_FOUND });
            }

            var materialResponse = _mapper.Map<MaterialResponse>(material);

            var shapedMaterial = _dataShapingService.ShapeData(materialResponse, request.Fields);

            return shapedMaterial;
        }
    }
}
