using FileUploader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private AwsUpload _awsUpload;

        public IndexModel(ILogger<IndexModel> logger, AwsUpload awsUpload)
        {
            _logger = logger;
            _awsUpload = awsUpload;
        }

        [BindProperty]
        public IFormFile FormFile { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var doNotSave = FileUploader.FileUploader.Upload(FormFile, "", "/images", "", 1);

            var isImage = FormFile.IsImage();

            // upload iFormFile
            var info1 = FormFile.GetImageInfo();
            var uploadFormFile = FileUploader.FileUploader.Upload(FormFile, "", "/images", "iFormFile.jpg");
            var resize1 = FormFile.ResizeImage(300, 200, "webp");
            var uploadResize1 = FileUploader.FileUploader.Upload(resize1, "", "/images", "resize1.webp");

            using (var ms = new MemoryStream())
            {
                await FormFile.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var info2 = fileBytes.GetImageInfo();
                var uploadFileBytes = FileUploader.FileUploader.Upload(fileBytes, Path.GetExtension(FormFile.FileName), "", "/images", "ByteSaved.jpg");
                var resize2 = fileBytes.ResizeImage(300, 200, "webp");
                var uploadResize2 = FileUploader.FileUploader.Upload(resize2, "", "/images", "resize2.webp");

                

                var base64String = Convert.ToBase64String(fileBytes);

                var info3 = base64String.GetImageInfo();
                var uploadBase64String = FileUploader.FileUploader.Upload(base64String, "", "/images", "Base64Saved.jpg");
                var resize3 = fileBytes.ResizeImage(300, 200, "webp");
                var uploadResize3 = FileUploader.FileUploader.Upload(resize3, "", "/images", "resize3.webp");
                
                //Aws
                var awsByte = await _awsUpload.UploadFile(fileBytes, "ByteSaved.jpg");
                var awsBase64 = await _awsUpload.UploadFile(base64String, "Base64Saved.jpg");
                var forDelete = await _awsUpload.UploadFile(base64String, "ForDelete.jpg");

                var pathSave = await _awsUpload.UploadFileFromFolder($"{Directory.GetCurrentDirectory()}/wwwroot/images/Base64Saved.jpg", "pathSave.jpg");

                var url = _awsUpload.GetFileUrl("ByteSaved.jpg", 1);
                var dl =await _awsUpload.DownloadFile("ByteSaved.jpg");
                var delete =await _awsUpload.DeleteFile("ForDelete.jpg");
            }


            return Page();
        }
        
    }
}
