using Microsoft.Extensions.Options;

namespace ConfigChecker.Services
{
    public class FileUploadService(IOptions<FileUploadServiceOptions> options) : IFileUploadService
    {
        private readonly long _maxFileSize = options.Value.MaxFileSize;
        private readonly string[] _allowedExtensions = options.Value.AllowedExtensions;

        public async ValueTask<string> ReadFormFileAsync(IFormFile formFile)
        {
            var checkedFile = await FormFileUtilities.ValidateFormFile(formFile, _maxFileSize, _allowedExtensions);

            if (checkedFile is null)
            {
                return string.Empty;
            }

            var safePath = Path.GetTempFileName();
            using var stream = File.Create(safePath);
            await checkedFile.CopyToAsync(stream);

            return safePath;
        }
    }
}
