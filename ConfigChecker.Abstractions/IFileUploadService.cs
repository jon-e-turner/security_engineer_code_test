using Microsoft.AspNetCore.Http;

namespace ConfigChecker.Abstractions
{
    public interface IFileUploadService
    {
        ValueTask<string> ReadFormFileAsync(IFormFile formFile);
    }
}
