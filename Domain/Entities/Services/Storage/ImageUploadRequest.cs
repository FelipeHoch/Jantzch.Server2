namespace Jantzch.Server2.Domain.Entities.Services.Storage;

public class ImageUploadRequest
{
    public string Id { get; set; }
    public string FileName { get; private set; }
    public Stream ImageStream { get; private set; }
    public string ContentType { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }

    private ImageUploadRequest() { }

    public static ImageUploadRequest Create(
        string id,
        string fileName,
        Stream imageStream,
        string contentType,
        Dictionary<string, string> metadata = null)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));

        if (imageStream == null || !imageStream.CanRead)
            throw new ArgumentException("Invalid image stream", nameof(imageStream));

        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be empty", nameof(contentType));

        return new ImageUploadRequest
        {
            Id = id,
            FileName = fileName,
            ImageStream = imageStream,
            ContentType = contentType,
            Metadata = metadata ?? new Dictionary<string, string>()
        };
    }
}
