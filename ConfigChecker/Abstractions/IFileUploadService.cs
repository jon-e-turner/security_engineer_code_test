namespace ConfigChecker.Abstractions
{
  public interface IFileUploadService
  {
    ValueTask<string> ReadFormFileAsync(IFormFile formFile);
  }
}
