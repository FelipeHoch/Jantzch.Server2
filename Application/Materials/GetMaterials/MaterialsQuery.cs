using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Application.Helpers;

namespace Jantzch.Server2.Features.Materials;

public class MaterialsResourceParameters : ResourceParameters
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Group { get; set; }    
}

public record MaterialsQuery(MaterialsResourceParameters MaterialsResourceParameters) : IRequest<PagedList<MaterialDTO>>;
