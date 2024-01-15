using FluentValidation;
using Jantzch.Server2.Infraestructure;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Materials.DeleteMaterial;

public class DeleteMaterialCommand
{
    public record Command(string Id) : IRequest;
}
