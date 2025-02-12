using ConfigChecker.Abstractions;
using Microsoft.Extensions.Options;

namespace ConfigChecker.Services
{
    public class FileUploadService(IOptions<FileUploadServiceOptions> options) : IFileUploadService
    {
        private readonly long _maxFileSize = options.Value.MaxFileSize;
        private readonly string[] _allowedExtensions = options.Value.AllowedExtensions;

        public async ValueTask<string> ReadFormFileAsync(IFormFile formFile)
        {
            if (!await formFile.TryValidateFormFile(_maxFileSize, _allowedExtensions))
            {
                return string.Empty;
            }

            var safePath = Path.GetTempFileName();
            using var stream = File.Create(safePath);
            await formFile.CopyToAsync(stream);

            return safePath;
        }
    }
}
