using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using MediatR;

namespace Jantzch.Server2.Application.Materials.GetMaterials;

public class GetMaterialsHandler
{
    public class QueryHandler : IRequestHandler<MaterialsQuery, PagedList<MaterialDTO>>
    {
        private readonly IMaterialsRepository _materialsRepository;

        private readonly IPaginationService _paginationService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public QueryHandler(IMaterialsRepository materialsRepository, IPaginationService paginationService, IHttpContextAccessor httpContextAccessor)
        {
            _materialsRepository = materialsRepository;

            _paginationService = paginationService;

            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedList<MaterialDTO>> Handle(MaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _materialsRepository.GetMaterials(request.MaterialsResourceParameters);

            _paginationService.AddPaginationMetadataInResponseHeader(materials, _httpContextAccessor.HttpContext.Response);

            return materials;
        }
    }
}
