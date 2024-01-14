using FluentValidation;
using Jantzch.Server2.Domain;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System.Net;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Features.Materials;

public class Create
{
    public class MaterialData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("eu")]
        public string Eu { get; set; }

        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("groupId")]
        public string? GroupId { get; set; }
    }

    public class MaterialDataValidator : AbstractValidator<MaterialData>
    {
        public MaterialDataValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Value).NotNull().NotEmpty();
            RuleFor(x => x.Eu).NotNull().NotEmpty();
            RuleFor(x => x.CreatedBy).NotNull().NotEmpty();
            RuleFor(x => x.GroupId).NotNull().NotEmpty().Must(x => ObjectId.TryParse(x, out _));
        }
    }

    public record Command(MaterialData Material) : IRequest<MaterialEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Material).NotNull().SetValidator(new MaterialDataValidator());
        }
    }

    public class Handler : IRequestHandler<Command, MaterialEnvelope>
    {
        private readonly JantzchContext _context;

        public Handler(JantzchContext context)
        {
            _context = context;
        }

        public async Task<MaterialEnvelope> Handle(Command request, CancellationToken cancellationToken)
        {
            var material = new Material
            {
                Id = ObjectId.GenerateNewId(),
                Name = request.Material.Name,
                Value = request.Material.Value,
                Eu = request.Material.Eu,
                CreatedBy = request.Material.CreatedBy,
                GroupIdObject = ObjectId.Parse(request.Material.GroupId)
            };

            var groupIdToCheck = request.Material.GroupId ?? string.Empty;

            var group = await _context.GroupMaterials
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id.ToString() == request.Material.GroupId, cancellationToken);

            if (group is null)
                throw new RestException(HttpStatusCode.NotFound, new { Group = "Not found" });


            await _context.Materials.AddAsync(material, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new MaterialEnvelope(material);
        }
    }
}
