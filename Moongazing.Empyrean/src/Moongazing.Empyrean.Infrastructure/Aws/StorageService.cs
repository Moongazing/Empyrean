using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Doing.Retail.Infrastructure.Aws.Services;
using Microsoft.Extensions.Options;
using Moongazing.Empyrean.Infrastructure.Aws.ConfigurationModels;

namespace Moongazing.Empyrean.Infrastructure.Aws;

public class StorageService : IStorageService
{
    private readonly AwsS3Settings awsS3Settings;
    private readonly IAmazonS3 s3Client;
    private readonly TransferUtility transferUtility;

    public StorageService(IOptions<AwsS3Settings> awsS3Settings,
                          IAmazonS3 s3Client)
    {
        this.awsS3Settings = awsS3Settings.Value;
        this.s3Client = s3Client;
        transferUtility = new(s3Client);
    }

    public async Task<string> UploadFileAsync(byte[] fileData, string category, Guid userId)
    {
        var fileName = $"{Guid.NewGuid()}";
        var key = GenerateFileKey(fileName, category, userId);

        using var stream = new MemoryStream(fileData);
        var request = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = key,
            BucketName = awsS3Settings.BucketName,
            CannedACL = S3CannedACL.PublicRead
        };

        await transferUtility.UploadAsync(request).ConfigureAwait(false);

        return GenerateFileUrl(key);
    }

    public async Task<string> UpdateFileAsync(string key, byte[] fileData, string category, Guid userId)
    {
        await DeleteFileAsync(key).ConfigureAwait(false);
        return await UploadFileAsync(fileData, category, userId).ConfigureAwait(false);
    }

    public async Task<bool> DeleteFileAsync(string key)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = awsS3Settings.BucketName,
            Key = key
        };

        var response = await s3Client.DeleteObjectAsync(deleteObjectRequest).ConfigureAwait(false);
        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    private static string GenerateFileKey(string fileName, string category, Guid userId)
    {
        return $"{category}/{Guid.NewGuid()}_{fileName}_{userId}";
    }

    private string GenerateFileUrl(string key)
    {
        return $"{awsS3Settings.ServiceURL}/{awsS3Settings.BucketName}/{key}";
    }

    public string ExtractFileKeyFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.AbsolutePath[(uri.AbsolutePath.LastIndexOf('/') + 1)..];
    }


    public async Task<ICollection<string>> ListFilesAsync(string category, Guid userId)
    {
        var prefix = $"{category}/";
        var result = new List<string>();

        var request = new ListObjectsV2Request
        {
            BucketName = awsS3Settings.BucketName,
            Prefix = prefix
        };

        ListObjectsV2Response response;
        do
        {
            response = await s3Client.ListObjectsV2Async(request).ConfigureAwait(false);

            foreach (var obj in response.S3Objects)
            {
                if (obj.Key.Contains(userId.ToString()))
                {
                    result.Add(GenerateFileUrl(obj.Key));
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);

        return result;
    }


    public async Task<List<string>> GetFilesByCategoryAsync(string category)
    {
        var prefix = $"{category}/"; 
        var result = new List<string>();

        var request = new ListObjectsV2Request
        {
            BucketName = awsS3Settings.BucketName,
            Prefix = prefix 
        };

        ListObjectsV2Response response;
        do
        {
            response = await s3Client.ListObjectsV2Async(request).ConfigureAwait(false);

            foreach (var obj in response.S3Objects)
            {
                result.Add(GenerateFileUrl(obj.Key)); 
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated); 

        return result;
    }


    public async Task<byte[]> GetFileByIdAsync(string fileId)
    {
        
        var prefix = $"{fileId}";

        var request = new ListObjectsV2Request
        {
            BucketName = awsS3Settings.BucketName,
            Prefix = prefix 
        };

        var response = await s3Client.ListObjectsV2Async(request).ConfigureAwait(false);

        if (response.S3Objects.Count == 0)
        {
            throw new Exception("File not found.");
        }

        var key = response.S3Objects.First().Key;

        var getObjectRequest = new GetObjectRequest
        {
            BucketName = awsS3Settings.BucketName,
            Key = key
        };

        using var getObjectResponse = await s3Client.GetObjectAsync(getObjectRequest).ConfigureAwait(false);
        using var memoryStream = new MemoryStream();
        await getObjectResponse.ResponseStream.CopyToAsync(memoryStream);

        return memoryStream.ToArray(); 
    }
    public async Task<List<string>> GetFilesByUserIdAsync(Guid userId)
    {
        var prefix = $"{userId}/"; 
        var result = new List<string>();

        var request = new ListObjectsV2Request
        {
            BucketName = awsS3Settings.BucketName,
            Prefix = prefix 
        };

        ListObjectsV2Response response;
        do
        {
            response = await s3Client.ListObjectsV2Async(request).ConfigureAwait(false);

            foreach (var obj in response.S3Objects)
            {
                result.Add(GenerateFileUrl(obj.Key)); 
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated); 

        return result;
    }

}
