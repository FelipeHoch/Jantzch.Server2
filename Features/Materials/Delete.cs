using FluentValidation;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Features.Materials;

public class Delete
{
    public record Command(string Id) : IRequest;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly JantzchContext _context;

        public Handler(JantzchContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var material = await _context.Materials.FindAsync(ObjectId.Parse(request.Id));

            if (material == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Material = "Not found" });
            }

            _context.Materials.Remove(material);

            await _context.SaveChangesAsync(cancellationToken);

            await Task.FromResult(Unit.Value);
        }
    }   
}
