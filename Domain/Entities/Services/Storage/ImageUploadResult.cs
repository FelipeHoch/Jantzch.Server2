namespace Jantzch.Server2.Domain.Entities.Services.Storage;

public class ImageUploadResult
{
    public string ImageKey { get; private set; }
    public string ImageUrl { get; private set; }
    public bool Success { get; private set; }
    public string ErrorMessage { get; private set; }
    public string Id { get; set; }

    private ImageUploadResult() { }

    public static ImageUploadResult CreateSuccess(string imageKey, string imageUrl, string id)
    {
        return new ImageUploadResult
        {
            ImageKey = imageKey,
            ImageUrl = imageUrl,
            Success = true,
            Id = id
        };
    }

    public static ImageUploadResult CreateFailure(string errorMessage)
    {
        return new ImageUploadResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
