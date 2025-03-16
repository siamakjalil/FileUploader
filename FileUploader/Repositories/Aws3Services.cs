using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FileUploader.Contracts;
using FileUploader.Models;

namespace FileUploader.Repositories
{
    public class Aws3Services : IAws3Services
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        public Aws3Services()
        {
            
        }
        public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey, string endPoint, string bucketName)
        {
            _bucketName = bucketName;
            _awsS3Client = new AmazonS3Client(new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey), new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ForcePathStyle = true,
                ServiceURL = endPoint,
                SignatureVersion = "v4",
            });
        }

        public async Task<byte[]> DownloadFileAsync(string file)
        {
            MemoryStream ms = null;

            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = file
            };

            using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                return null;

            return ms.ToArray();
        }

        public string GetFileUrlAsync(string file, int timeout = 15)
        {
            var getObjectRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = file,
                Expires = DateTime.Now.AddHours(1).AddMinutes(timeout),

            };
            var request = _awsS3Client.GetPreSignedURL(getObjectRequest);
            return request;
        }

        public async Task<PutObjectResponse> UploadFileAsync(AddFile input)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{input.Key}",
                FilePath = input.File,
                CannedACL = S3CannedACL.PublicRead,

            };
            var response = await _awsS3Client.PutObjectAsync(request);
            return response;
        }
        public async Task<PutObjectResponse> UploadBas64Async(AddFile input)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{input.Key}",
                CannedACL = S3CannedACL.PublicRead,
            };
            if (string.IsNullOrEmpty(input.File)) return null;
            var bytes = Convert.FromBase64String(input.File);
            using var ms = new MemoryStream(bytes);
            request.InputStream = ms;
            var response = await _awsS3Client.PutObjectAsync(request);
            return response;
        }

        public async Task<PutObjectResponse> UploadBas64SHA256Async(AddFile input)
        {
            if (string.IsNullOrEmpty(input.File)) return null;
            var bytes = Convert.FromBase64String(input.File);
            string sha256Hash;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);
                sha256Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{input.Key}",
                CannedACL = S3CannedACL.PublicRead,
            };
            request.Headers["x-amz-content-sha256"] = sha256Hash;
            var response = await _awsS3Client.PutObjectAsync(request);
            return response;
        }

        public async Task<PutObjectResponse> UploadByteAsync(AddFile input)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{input.Key}",
                CannedACL = S3CannedACL.PublicRead,

            };
            using var ms = new MemoryStream(input.Byte);
            request.InputStream = ms;
            var response = await _awsS3Client.PutObjectAsync(request);
            return response;
        }

        public async Task<DeleteObjectResponse> DeleteFileAsync(string fileName, string versionId = "")
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            if (!string.IsNullOrEmpty(versionId))
                request.VersionId = versionId;

            return await _awsS3Client.DeleteObjectAsync(request);
        }
    }
}
