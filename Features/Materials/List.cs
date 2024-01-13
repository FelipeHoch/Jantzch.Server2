using Jantzch.Server2.Infraestructure;
using MediatR;

namespace Jantzch.Server2.Features.Materials;

public class List
{
    public record Query() : IRequest<MaterialsSet>;

    public class QueryHandler : IRequestHandler<Query, MaterialsSet>
    {
        private readonly JantzchContext _context;
    }
}
