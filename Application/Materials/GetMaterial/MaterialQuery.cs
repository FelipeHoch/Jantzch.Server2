using Jantzch.Server2.Features.Materials;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Materials.GetMaterial;

public record MaterialQuery(string Id, string? Fields) : IRequest<ExpandoObject>;
