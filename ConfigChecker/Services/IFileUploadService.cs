namespace ConfigChecker.Services
{
  public interface IFileUploadService
  {
    ValueTask<string> ReadFormFileAsync(IFormFile formFile);
  }
}
