using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Core;

public class ImageService
{
    private const string ImageContainerName = "images";
    private readonly BlobContainerClient _blobContainerClient;
    private readonly Uri ImageContainerUri;

    public ImageService(IConfiguration config, TokenCredential credential)
    {
        var blobServiceEndpoint = config["BlobServiceEndpoint"];
        var imageServiceUri = new Uri(blobServiceEndpoint);
        ImageContainerUri = new Uri(imageServiceUri, ImageContainerName);
        _blobContainerClient = new BlobContainerClient(ImageContainerUri, credential);
    }

    public Task UploadAsync(String path, Stream stream) =>
        _blobContainerClient.UploadBlobAsync(DateTimeString() + path, stream);

    private string DateTimeString() => DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    public ValueTask<List<Uri>> ListAsync() =>
        _blobContainerClient
            .GetBlobsAsync()
            .OrderByDescending(blob => blob.Properties.CreatedOn)
            .Select(ToAbsoluteUri)
            .ToListAsync();

    private Uri ToAbsoluteUri(BlobItem blob) =>
        new Uri(ImageContainerUri, $"{ImageContainerName}/{blob.Name}");

}