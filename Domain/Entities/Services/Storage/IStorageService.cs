namespace Jantzch.Server2.Domain.Entities.Services.Storage;

public interface IStorageService
{
    Task<ImageUploadResult> UploadImageAsync(ImageUploadRequest request);
    Task<IEnumerable<ImageUploadResult>> UploadImagesAsync(IEnumerable<ImageUploadRequest> requests);
    Task<bool> DeleteImageAsync(string imageKey);
    Task<IEnumerable<bool>> DeleteImagesAsync(IEnumerable<string> imageKeys);
}
