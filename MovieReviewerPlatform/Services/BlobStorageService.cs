using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using MovieReviewerPlatform.Contracts.Interfaces;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IConfiguration configuration)
    {
        string connectionString = configuration.GetSection("AzureStorage:ConnectionString").Value;
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadFileAsync(string containerName, string blobName, Stream fileStream)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(fileStream, true);

        // Return the URL of the uploaded image
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.OpenReadAsync();
    }

    public async Task<bool> DeleteFileAsync(string containerName, string blobName)
    {
        // Get the container client
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        // Check if the container exists
        if (!await containerClient.ExistsAsync())
        {
            return false;
        }

        // Get the blob client for the specified blob
        var blobClient = containerClient.GetBlobClient(blobName);

        // Check if the blob exists
        if (!await blobClient.ExistsAsync())
        {
            return false;
        }

        // Delete the blob
        await blobClient.DeleteAsync();

        return true;
    }

}
