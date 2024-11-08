using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Services.Storage;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Deals;

public class UploadImages 
{
    public class UploadImageRequest
    {
        public ImageKeyEnum Key { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();
    }

    public record Command(string DealId, List<UploadImageRequest> UploadImages) : IRequest;

    public class Handlder(
        IDealRepository dealRepository,
        IStorageService storageService
    ) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken);

            if (deal is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });
            }

            var imagesToUpload = request.UploadImages.Select(image =>
            {              
                if (image.Image is null or { Length: 0 })
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { message = DealErrorMessages.ERROR_CREATE });
                }

                var upload = ImageUploadRequest.Create(
                    image.Id,
                    image.Image.FileName,
                    image.Image.OpenReadStream(),
                    image.Image.ContentType
                );

                return upload;
            });          

            var uploadedImages = await storageService.UploadImagesAsync(imagesToUpload);

            uploadedImages.ToList().ForEach(img =>
            {
                var imageData = request.UploadImages.Find(upload => upload.Id == img.Id);

                var image = Image.Create(
                    img.Id,
                    img.ImageUrl,
                    imageData.Description,
                    imageData.Key
                );

                deal.AddImage(image);
            });

            await dealRepository.UpdateAsync(deal, cancellationToken);
        }
    }
}
