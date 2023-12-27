using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploader.Models
{
    public class BucketConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string EndPoint { get; set; }
    }
}
