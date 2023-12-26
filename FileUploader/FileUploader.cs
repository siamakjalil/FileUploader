using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace FileUploader
{
    public static class FileUploader
    {
        /// <summary>
        /// for base64
        /// </summary>
        /// <param name="file">base64 file</param>
        /// <param name="oldFileName">old fileName if exist would be removed</param>
        /// <param name="path">path to upload , example : /images/users </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(string file, string oldFileName, string path,long maxSize = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    return oldFileName;
                }
                var extension = file.Split(';')[0].Split('/')[1];
                var fileName = $"{Guid.NewGuid()}.{extension}";
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                var newFilePath = $"{savePath}/{fileName}";
                if (file.Contains("data"))
                {
                    var files = file.Split(",");
                    file = files[1];
                }
                if (file.StartsWith(','))
                {
                    file = file.Substring(1, file.Length - 1);
                }

                var fileByte = Convert.FromBase64String(file);

                if (maxSize!=0)
                {
                    if (!IsAllowedLength(fileByte.Length, maxSize))
                    {
                        return oldFileName;
                    }

                }

                File.WriteAllBytes(newFilePath, fileByte);

                if (string.IsNullOrEmpty(oldFileName)) return $"{fileName}";
                var deletePath = $"{savePath}/{oldFileName}";
                if (File.Exists(deletePath))
                    File.Delete(deletePath);
                return $"{fileName}";
            }
            catch (Exception e)
            {
                return oldFileName;
            }
        }
        /// <summary>
        /// for ByteArray
        /// </summary>
        /// <param name="file">ByteArray file</param>
        /// <param name="extension">file extension </param>
        /// <param name="oldFileName">old fileName if exist would be removed</param>
        /// <param name="path">path to upload , example : /images/users </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(byte[] file ,string extension, string oldFileName, string path,long maxSize = 0)
        {
            try
            {
                if (file.Length == 0)
                {
                    return oldFileName;
                } 
                var fileName = $"{Guid.NewGuid()}.{extension}";
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                var newFilePath = $"{savePath}/{fileName}";

                if (maxSize != 0)
                {
                    if (!IsAllowedLength(file.Length, maxSize))
                    {
                        return oldFileName;
                    }

                }

                File.WriteAllBytes(newFilePath, file);

                if (string.IsNullOrEmpty(oldFileName)) return $"{fileName}";
                var deletePath = $"{savePath}/{oldFileName}";
                if (File.Exists(deletePath))
                    File.Delete(deletePath);
                return $"{fileName}";
            }
            catch (Exception e)
            {
                return oldFileName;
            }
        }
        /// <summary>
        /// for IFormFile
        /// </summary>
        /// <param name="file">IFormFile</param>
        /// <param name="oldFileName">old fileName if exist would be removed</param>
        /// <param name="path">path to upload , example : /images/users </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(IFormFile file, string oldFileName, string path,long maxSize = 0)
        {
            try
            {
                if (file == null)
                {
                    return oldFileName;
                }
                var ex = Path.GetExtension(file.FileName);
                if (maxSize != 0)
                {
                    if (!IsAllowedLength(file.Length, maxSize))
                    {
                        return oldFileName;
                    }
                }
                var fileName = $"{Guid.NewGuid()}.{ex}"; 
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                var newFilePath = $"{savePath}/{fileName}";
                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (string.IsNullOrEmpty(oldFileName)) return $"{fileName}";
                var deletePath = $"{savePath}/{oldFileName}";
                if (File.Exists(deletePath))
                    File.Delete(deletePath);
                return $"{fileName}";
            }
            catch (Exception e)
            {
                return oldFileName;
            }
        }

        private static bool IsAllowedLength(long len , long allowed)
        {
            var mb = (len / 1024f) / 1024f;
            return mb <= allowed;
        }
    }
}
