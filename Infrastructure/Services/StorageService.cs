using Amazon.S3;
using Jantzch.Server2.Domain.Entities.Services.Storage;
using Microsoft.Extensions.Options;
using Amazon.S3.Model;
using Jantzch.Server2.Infrastructure.Configuration;

namespace Jantzch.Server2.Infrastructure.Services;

public class StorageService(
    IAmazonS3 s3Client,
    IOptions<S3Settings> settings,
    ILogger<StorageService> logger) : IStorageService
{
    private readonly IAmazonS3 _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    private readonly S3Settings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    private readonly ILogger<StorageService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<ImageUploadResult> UploadImageAsync(ImageUploadRequest request)
    {
        try
        {
            var uniqueId = request.Id;

            var imageKey = GenerateImageKey(request.FileName, uniqueId);

            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = imageKey,
                InputStream = request.ImageStream,
                ContentType = request.ContentType,                
            };

            await _s3Client.PutObjectAsync(putRequest);

            var imageUrl = GenerateImageUrl(imageKey);

            return ImageUploadResult.CreateSuccess(uniqueId, imageUrl, uniqueId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image {FileName}", request.FileName);
            return ImageUploadResult.CreateFailure(ex.Message);
        }
    }

    public async Task<IEnumerable<ImageUploadResult>> UploadImagesAsync(IEnumerable<ImageUploadRequest> requests)
    {
        var uploadTasks = requests.Select(UploadImageAsync);
        return await Task.WhenAll(uploadTasks);
    }

    public async Task<bool> DeleteImageAsync(string imageKey)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = imageKey
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image {ImageKey}", imageKey);
            return false;
        }
    }

    public async Task<IEnumerable<bool>> DeleteImagesAsync(IEnumerable<string> imageKeys)
    {
        var deleteTasks = imageKeys.Select(DeleteImageAsync);
        return await Task.WhenAll(deleteTasks);
    }

    private string GenerateImageKey(string fileName, string uniqueId)
    {        
        var sanitizedFileName = SanitizeFileName(fileName);
        return $"{_settings.FolderPrefix}/{uniqueId}/{sanitizedFileName}";
    }

    private string GenerateImageUrl(string imageKey)
    {
        return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{imageKey}";
    }

    private string SanitizeFileName(string fileName)
    {
        // Remove invalid characters and spaces
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars));
        return sanitized.Replace(" ", "-").ToLowerInvariant();
    }
}
