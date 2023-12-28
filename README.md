# FileUploader
This package helps us to upload our file easily on local server or aws server.
And also we can work with image tools that explain blew.

## CheckImageContent
This class check image content and inform us if image has bad content.
```
IsImage(IFormFile postedFile)
```
## ImageTools
This class use for getting images info , resizing them or changing format.
```
// Get size , width and height from these methods:
GetImageInfo(string imgFile) // for base64
GetImageInfo(byte[] imgFile) // for byteArray
GetImageInfo(IFormFile imgFile) // for IFormFile

// These methods can resize image
// Format can be (webp , jpeg , png) to get result as base64
ResizeImage(string imgFile, int width, int height, string format) // for base64
ResizeImage(byte[] imgFile, int width, int height, string format) // for byteArray
ResizeImage(IFormFile imgFile, int width, int height, string format) // for IFormFile

// These methods changes images format 
// Format can be (webp , jpeg , png) to get result as base64
ChangeImageFormat(string imgFile, string format)
ChangeImageFormat(byte[] imgFile, string format)
ChangeImageFormat(IFormFile imgFile, string format)
```
## FileUploader
FileUploader class for save files on local server.
```
// if fileName is empty methods generate fileName by guid 
// if maxSize is not zero methods check file size before save
Upload(string file, string oldFileName, string path,string fileName = "",long maxSize = 0) // save base64
Upload(byte[] file ,string extension, string oldFileName, string path, string fileName = "", long maxSize = 0) // save byteArray
Upload(IFormFile file, string oldFileName, string path, string fileName = "", long maxSize = 0) // save IFormFile
```
## AwsUpload
This class use for upload files on AWS buckets.

Usage :
In the "Config" folder and FileUploaderServicesRegistration we have BucketConfigs that they read from appsettings on your project like following codes:
```
"BucketConfigs": {
    "AccessKey": "<YOUR_ACCESSKEY>",
    "SecretKey": "<YOUR_SECRETKEY>",
    "EndPoint": "<YOUR_ENDPOINT>",
    "BucketName": "<YOUR_BUCKETNAME>"
  }
```
Add this config to your program.cs 
```
builder.Services.ConfigureFileUploaderServices(builder.Configuration);
```
methods:
```
// key is file name 

UploadFile(string fileBase64, string key) // upload base64 
UploadFile(byte[] file, string key) // upload byteArray

UploadFileFromFolder(string path, string key) // upload file from path

GetFileUrl(string key, int timeOut = 15) // get file url with limitation , default is 15 min

DownloadFile(string key) // download file as byteArray 

DeleteFile(string key, string versionId = "") // delete file from bucket 
```