using System.Threading.Tasks;
using Amazon.S3.Model;
using FileUploader.Models;

namespace FileUploader.Contracts
{
    public interface IAws3Services
    {
        Task<byte[]> DownloadFileAsync(string file);
        string GetFileUrlAsync(string file, int timeout = 15);

        Task<PutObjectResponse> UploadFileAsync(AddFile input);
        Task<PutObjectResponse> UploadBas64Async(AddFile input);
        Task<PutObjectResponse> UploadBas64SHA256Async(AddFile input);
        Task<PutObjectResponse> UploadByteAsync(AddFile input);

        Task<DeleteObjectResponse> DeleteFileAsync(string fileName, string versionId = "");
    }
}
