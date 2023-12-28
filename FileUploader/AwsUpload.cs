using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FileUploader.Contracts;
using FileUploader.Models;
using FileUploader.Repositories;
using Microsoft.Extensions.Options;

namespace FileUploader
{
    public class AwsUpload
    {
        private readonly IAws3Services _aws3Services;

        public AwsUpload(IOptions<BucketConfig> bucketConfig)
        {
            _aws3Services = new Aws3Services(
                bucketConfig.Value.AccessKey,
                             bucketConfig.Value.SecretKey,
                             bucketConfig.Value.EndPoint,
                             bucketConfig.Value.BucketName);
        }
        /// <summary>
        /// Upload file to aws server as base64
        /// </summary>
        /// <param name="fileBase64">Base64 file</param>
        /// <param name="key">File name </param>
        /// <returns></returns>
        public async Task<AwsResponseMessage> UploadFile(string fileBase64, string key)
        {
            try
            {
                var res = await _aws3Services.UploadBas64Async(new AddFile()
                {
                    File = fileBase64,
                    Key = key
                });
                return new AwsResponseMessage()
                {
                    Status = res.HttpStatusCode
                };

            }
            catch (Exception e)
            {
                return new AwsResponseMessage()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }
        /// <summary>
        /// Upload file to aws server as byteArray
        /// </summary>
        /// <param name="file">ByteArray file</param>
        /// <param name="key">File name </param>
        /// <returns></returns>
        public async Task<AwsResponseMessage> UploadFile(byte[] file, string key)
        {
            try
            {
                var res = await _aws3Services.UploadByteAsync(new AddFile()
                {
                    Byte = file,
                    Key = key
                });
                return new AwsResponseMessage()
                {
                    Status = res.HttpStatusCode
                };

            }
            catch (Exception e)
            {
                return new AwsResponseMessage()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }
        /// <summary>
        /// Upload file to aws server from folder
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="key">File name </param>
        /// <returns></returns>
        public async Task<AwsResponseMessage> UploadFileFromFolder(string path, string key)
        {
            try
            {
                var res = await _aws3Services.UploadFileAsync(new AddFile()
                {
                    File = path,
                    Key = key
                });
                return new AwsResponseMessage()
                {
                    Status = res.HttpStatusCode
                };

            }
            catch (Exception e)
            {
                return new AwsResponseMessage()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }
        /// <summary>
        /// Get file url with limitation
        /// </summary>
        /// <param name="key">File name</param>
        /// <param name="timeOut">url timeOut </param>
        /// <returns></returns>
        public string GetFileUrl(string key, int timeOut = 15)
        {
            var res = _aws3Services.GetFileUrlAsync(key, timeOut);
            return res;
        }
        /// <summary>
        /// Download file as byteArray
        /// </summary>
        /// <param name="key">File name</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadFile(string key)
        {
            var res = await _aws3Services.DownloadFileAsync(key);
            return res;
        }
        /// <summary>
        /// Delete file from aws 
        /// </summary>
        /// <param name="key">File name</param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public async Task<AwsResponseMessage> DeleteFile(string key, string versionId = "")
        {
            try
            {
                var res = await _aws3Services.DeleteFileAsync(key, versionId);
                return new AwsResponseMessage()
                {
                    Status = res.HttpStatusCode
                };
            }
            catch (Exception e)
            {
                return new AwsResponseMessage()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }
    }
}
