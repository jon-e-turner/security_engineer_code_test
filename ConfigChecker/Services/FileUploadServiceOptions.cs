namespace ConfigChecker.Services
{
    public class FileUploadServiceOptions
    {
        public string[ ] AllowedExtensions = [];
        public long MaxFileSize = 16777216; // 16Mb
    }
}
