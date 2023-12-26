using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileUploader.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace FileUploader
{
    public static class ImageTools
    {
        /// <summary>
        /// Get image info(size,width and height) from base64 image
        /// </summary>
        /// <param name="imgFile"> Base64 image </param>
        /// <returns>
        /// ImageInfo as model
        /// </returns>
        public static ImageInfo GetImageInfo(this string imgFile)
        {
            var img = Convert.FromBase64String(imgFile);
            var image = Image.Load(img);
            return new ImageInfo()
            {
                Height = image.Height,
                Size = (img.Length / 1024f) / 1024f,
                Width = image.Width,
            };
        }
        /// <summary>
        /// Get image info(size,width and height) from byteArray image
        /// </summary>
        /// <param name="imgFile"> ByteArray image </param>
        /// <returns>
        /// ImageInfo as model
        /// </returns>
        public static ImageInfo GetImageInfo(this byte[] imgFile)
        {
            var img = imgFile;
            var image = Image.Load(img);
            return new ImageInfo()
            {
                Height = image.Height,
                Size = (img.Length / 1024f) / 1024f,
                Width = image.Width,
            };
        }
        /// <summary>
        /// Get image info(size,width and height) from IFormFile image
        /// </summary>
        /// <param name="imgFile"> IFormFile image </param>
        /// <returns>
        /// ImageInfo as model
        /// </returns>
        public static ImageInfo GetImageInfo(this IFormFile imgFile)
        {
            using var ms = new MemoryStream();
            imgFile.CopyTo(ms);
            var img = ms.ToArray();
            var image = Image.Load(img);
            return new ImageInfo()
            {
                Height = image.Height,
                Size = (img.Length / 1024f) / 1024f,
                Width = image.Width,
            };
        }

        /// <summary>
        /// Resize image and get it as base64
        /// </summary>
        /// <param name="imgFile">file as Base64 </param>
        /// <param name="width">new width</param>
        /// <param name="height">new height</param>
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ResizeImage(this string imgFile, int width, int height, string format)
        {
            var img = Convert.FromBase64String(imgFile);
            var image = Image.Load(img);
            if (width != 0 && height != 0)
            {
                image.Mutate(x => x.Resize(width, height));
            }

            return ChangeImageFormat(image, format);
        }
        /// <summary>
        /// Resize image and get it as base64
        /// </summary>
        /// <param name="imgFile">file as ByteArray </param>
        /// <param name="width">new width</param>
        /// <param name="height">new height</param>
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ResizeImage(this byte[] imgFile, int width, int height, string format)
        {
            var img = imgFile;
            var image = Image.Load(img);
            if (width != 0 && height != 0)
            {
                image.Mutate(x => x.Resize(width, height));
            }

            return ChangeImageFormat(image, format);
        }
        /// <summary>
        /// Resize image and get it as base64
        /// </summary>
        /// <param name="imgFile">file as IFormFile </param>
        /// <param name="width">new width</param>
        /// <param name="height">new height</param>
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ResizeImage(this IFormFile imgFile, int width, int height, string format)
        {
            using var ms = new MemoryStream();
            imgFile.CopyTo(ms);
            var img = ms.ToArray();
            var image = Image.Load(img);
            if (width != 0 && height != 0)
            {
                image.Mutate(x => x.Resize(width, height));
            }
            return ChangeImageFormat(image, format);
        }
        /// <summary>
        /// Change image format 
        /// </summary>
        /// <param name="imgFile">File as base64</param>
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ChangeImageFormat(this string imgFile, string format)
        {
            var img = Convert.FromBase64String(imgFile);
            var image = Image.Load(img);
            return ChangeImageFormat(image, format);
        }
        /// <summary>
        /// Change image format 
        /// </summary>
        /// <param name="imgFile">file as ByteArray </param> 
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ChangeImageFormat(this byte[] imgFile, string format)
        {
            var img = imgFile;
            var image = Image.Load(img);
            return ChangeImageFormat(image, format);
        }
        /// <summary>
        /// Change image format 
        /// </summary>
        /// <param name="imgFile">file as IFormFile </param> 
        /// <param name="format">Returned file format as webp , jpeg , png </param>
        /// <returns>Base64</returns>
        public static string ChangeImageFormat(this IFormFile imgFile, string format)
        {
            using var ms = new MemoryStream();
            imgFile.CopyTo(ms);
            var img = ms.ToArray();
            var image = Image.Load(img);
            return ChangeImageFormat(image, format);
        }

        private static string ChangeImageFormat(Image image, string format)
        {
            switch (format.ToLower())
            {
                case "webp":
                    return image.ToBase64String(WebpFormat.Instance);
                case "jpeg":
                case "jpg":
                    return image.ToBase64String(JpegFormat.Instance);
                case "png":
                    return image.ToBase64String(PngFormat.Instance);
                default:
                    return image.ToBase64String(JpegFormat.Instance);
            }
        }
    }
}
