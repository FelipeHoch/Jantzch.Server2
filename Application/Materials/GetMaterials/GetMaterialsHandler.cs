using AutoMapper;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.Materials.GetMaterials;

public class GetMaterialsHandler
{
    public class QueryHandler : IRequestHandler<MaterialsQuery, IEnumerable<ExpandoObject>>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IPaginationService _paginationService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        private readonly IPropertyCheckerService _propertyCheckerService;

        public QueryHandler(
            IMaterialsRepository materialsRepository,
            IPaginationService paginationService, 
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IPropertyCheckerService propertyCheckerService)
        {
            _materialsRepository = materialsRepository;

            _paginationService = paginationService;

            _httpContextAccessor = httpContextAccessor;

            _mapper = mapper;

            _propertyCheckerService = propertyCheckerService;
        }

        public async Task<IEnumerable<ExpandoObject>> Handle(MaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _materialsRepository.GetMaterials(request.MaterialsResourceParameters);

            _paginationService.AddPaginationMetadataInResponseHeader(materials, _httpContextAccessor.HttpContext.Response);

            var materialsList = materials.ToList();

            var materialsDto = _mapper.Map<IEnumerable<MaterialDTO>>(materialsList);

            if (!_propertyCheckerService.TypeHasProperties<MaterialDTO>(request.MaterialsResourceParameters.Fields))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Error = "The fields provided are not valid" });
            }

            var materialsShaped = materialsDto.ShapeData(request.MaterialsResourceParameters.Fields);

            return materialsShaped;
        }
    }
}
