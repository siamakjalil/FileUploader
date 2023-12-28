using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FileUploader.Models
{
    public class AwsResponseMessage
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
