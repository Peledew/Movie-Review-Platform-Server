namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, string blobName, Stream fileStream);
        Task<Stream> DownloadFileAsync(string containerName, string blobName);
        Task<bool> DeleteFileAsync(string containerName, string blobName);
    }
}
