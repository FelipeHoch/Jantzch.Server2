using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Application.Helpers;
using System.Dynamic;
using Jantzch.Server2.Application.Materials;

namespace Jantzch.Server2.Features.Materials;

public record MaterialsQuery(MaterialsResourceParameters MaterialsResourceParameters) : IRequest<IEnumerable<ExpandoObject>>;
