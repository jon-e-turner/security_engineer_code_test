namespace ConfigChecker.Services
{
  public class FileUploadServiceOptions
  {
    public long MaxFileSize = 16777216; // 16Mb 

    public string[] AllowedExtensions = { "txt", "json" };
  }
}
