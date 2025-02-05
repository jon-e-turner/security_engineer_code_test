namespace ConfigChecker.Services
{
  public interface IFileUploadService
  {
    ValueTask<string> ReadFormFile(IFormFile formFile);
  }
}
