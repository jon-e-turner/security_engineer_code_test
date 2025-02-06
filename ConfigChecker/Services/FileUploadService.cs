using Microsoft.Extensions.Options;

namespace ConfigChecker.Services
{
  public class FileUploadService(IOptions<FileUploadServiceOptions> options) : IFileUploadService
  {
    private readonly long _maxFileSize = options.Value.MaxFileSize;
    private readonly string[] _allowedExtensions = options.Value.AllowedExtensions;

    public async ValueTask<string> ReadFormFileAsync(IFormFile formFile)
    {
      if (!ValidateFormFile(formFile))
      {
        return string.Empty;
      }

      var safePath = Path.GetTempFileName();
      using var stream = File.Create(safePath);
      await formFile.CopyToAsync(stream);

      return safePath;
    }

    private bool ValidateFormFile(IFormFile formFile) => 
      formFile.Length > 0
        && formFile.Length <= _maxFileSize
        && _allowedExtensions.Contains(Path.GetExtension(formFile.FileName).ToLowerInvariant());
  }
}
