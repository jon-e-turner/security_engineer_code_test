using ConfigChecker.Abstractions;
using ConfigChecker.Utilities;

using Microsoft.Extensions.Options;

namespace ConfigChecker.Services
{
    public class FileUploadService(IOptions<FileUploadServiceOptions> options) : IFileUploadService
    {
        private readonly string[ ] _allowedExtensions = options.Value.AllowedExtensions;
        private readonly long _maxFileSize = options.Value.MaxFileSize;

        public async ValueTask<string> ReadFormFileAsync(IFormFile formFile)
        {
            if (!await formFile.TryValidateFormFile(_maxFileSize, _allowedExtensions))
            {
                return string.Empty;
            }

            var safePath = Path.GetTempFileName( );
            using var stream = File.Create(safePath);
            await formFile.CopyToAsync(stream);

            return safePath;
        }
    }
}
