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
        /// <param name="fileName"> if fileName is empty method generate new fileName </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(string file, string oldFileName, string path, string fileName = "", long maxSize = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    return oldFileName;
                }
                var extension = file.Split(';')[0].Split('/')[1];
                fileName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid()}.{extension}" : fileName;
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                FileDirectory.CheckExistsAndCreate(savePath);
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

                if (maxSize != 0)
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
        /// <param name="fileName"> if fileName is empty method generate new fileName </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(byte[] file, string extension, string oldFileName, string path, string fileName = "", long maxSize = 0)
        {
            try
            {
                if (file.Length == 0)
                {
                    return oldFileName;
                }
                fileName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid()}.{extension}" : fileName;
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                FileDirectory.CheckExistsAndCreate(savePath);
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
        /// <param name="fileName"> if fileName is empty method generate new fileName </param>
        /// <param name="maxSize">max size allowed to upload</param>
        /// <returns></returns>
        public static string Upload(IFormFile file, string oldFileName, string path, string fileName = "", long maxSize = 0)
        {
            try
            {
                if (file == null)
                {
                    return oldFileName;
                }
                var extension = Path.GetExtension(file.FileName);
                if (maxSize != 0)
                {
                    if (!IsAllowedLength(file.Length, maxSize))
                    {
                        return oldFileName;
                    }
                }
                fileName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid()}.{extension}" : fileName;
                var savePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                FileDirectory.CheckExistsAndCreate(savePath);
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
        /// <summary>
        /// Delete your file 
        /// </summary>
        /// <param name="path">Your file path</param>
        /// <returns>If is success return empty string else return error</returns>
        public static string Delete(string path)
        {
            try
            {
                var deletePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}";
                if (File.Exists(deletePath))
                    File.Delete(deletePath);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Delete your file 
        /// </summary>
        /// <param name="path">Your file path</param>
        /// <param name="fileName">Your file fileName</param>
        /// <returns>If is success return empty string else return error</returns>
        public static string Delete(string path, string fileName)
        {
            try
            {
                var deletePath = $"{Directory.GetCurrentDirectory()}/wwwroot{path}/{fileName}";
                if (File.Exists(deletePath))
                    File.Delete(deletePath);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static bool IsAllowedLength(long len, long allowed)
        {
            var mb = (len / 1024f) / 1024f;
            return mb <= allowed;
        }
    }
}
