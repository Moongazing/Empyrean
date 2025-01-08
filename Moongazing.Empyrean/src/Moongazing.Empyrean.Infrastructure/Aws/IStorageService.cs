namespace Doing.Retail.Infrastructure.Aws.Services;

public interface IStorageService

{
    Task<string> UploadFileAsync(byte[] fileData, string category, Guid userId);
    Task<ICollection<string>> ListFilesAsync(string category, Guid userId);
    string ExtractFileKeyFromUrl(string url);
    Task<bool> DeleteFileAsync(string key);
    Task<string> UpdateFileAsync(string key, byte[] fileData, string category, Guid userId);
}