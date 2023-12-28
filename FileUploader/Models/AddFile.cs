namespace FileUploader.Models
{
    public class AddFile
    {
        public string? Url { get; set; }
        public string Key { get; set; }
        public string? File { get; set; }
        public byte[]? Byte { get; set; }
    }
}
