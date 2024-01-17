using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupMaterial;

public class GetGroupMaterialHandler
{
    public class Handler : IRequestHandler<GroupMaterialQuery, ExpandoObject>
    {
        private readonly IGroupsMaterialRepository _groupMaterialRepository;

        private readonly IMapper _mapper;

        private readonly IDataShapingService _dataShapingService;

        public Handler(
            IGroupsMaterialRepository groupMaterialRepository, 
            IMapper mapper, 
            IDataShapingService dataShapingService
        )
        {
            _groupMaterialRepository = groupMaterialRepository;
            _mapper = mapper;
            _dataShapingService = dataShapingService;
        }

        public async Task<ExpandoObject> Handle(GroupMaterialQuery request, CancellationToken cancellationToken)
        {
            var groupMaterial = await _groupMaterialRepository.GetGroupByIdAsync(new ObjectId(request.Id), cancellationToken);

            if (groupMaterial is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { GroupMaterial = Constants.NOT_FOUND });
            }

            var groupMaterialResponse = _mapper.Map<GroupMaterialResponse>(groupMaterial);

            var shapedGroupMaterial = _dataShapingService.ShapeData(groupMaterialResponse, request.Fields);

            return shapedGroupMaterial;
        }
    } 
}
