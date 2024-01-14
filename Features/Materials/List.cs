using Jantzch.Server2.Domain;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jantzch.Server2.Features.Materials;

public class List
{
    public record Query() : IRequest<MaterialsEnvelope>;

    public class QueryHandler : IRequestHandler<Query, MaterialsEnvelope>
    {
        private readonly JantzchContext _context;

        public QueryHandler(JantzchContext context)
        {
            _context = context;
        }

        public async Task<MaterialsEnvelope> Handle(Query request, CancellationToken cancellationToken)
        {
            
            var materials = await _context.Materials
                .AsNoTracking()
                .ToListAsync(cancellationToken);
             

            
            return new MaterialsEnvelope
            {
                Materials = materials,
                MaterialsCount = materials.Count
            };
        }
    }
}
