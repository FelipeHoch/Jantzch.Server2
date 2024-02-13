using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Materials.GetMaterials;

public class GetMaterialsHandler
{
    public class QueryHandler : IRequestHandler<MaterialsQuery, IEnumerable<ExpandoObject>>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IPaginationService _paginationService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        private readonly IDataShapingService _dataShapingService;

        public QueryHandler(
            IMaterialsRepository materialsRepository,
            IPaginationService paginationService, 
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IDataShapingService dataShapingService
            )
        {
            _materialsRepository = materialsRepository;

            _paginationService = paginationService;

            _httpContextAccessor = httpContextAccessor;

            _mapper = mapper;

            _dataShapingService = dataShapingService;
        }

        public async Task<IEnumerable<ExpandoObject>> Handle(MaterialsQuery request, CancellationToken cancellationToken)
        {         
            var materials = await _materialsRepository.GetMaterialsAsync(request.MaterialsResourceParameters, cancellationToken);

            _paginationService.AddPaginationMetadataInResponseHeader(materials, _httpContextAccessor.HttpContext.Response);

            var materialsList = materials.ToList();

            var materialsDto = _mapper.Map<List<MaterialResponse>>(materialsList);          

            var materialsShaped = _dataShapingService.ShapeDataList(materialsDto, request.MaterialsResourceParameters.Fields);

            return materialsShaped;
        }
    }
}
