using System.Net;

namespace ConfigChecker.Utilities
{
  public static class FormFileUtilities
  {
    public static async Task<bool> TryValidateFormFile(this IFormFile formFile, long maxFileSize, string[] allowedFileExtensions)
    {
      // Check the file length. This check doesn't catch files that only have
      // a BOM as their content.
      ArgumentNullException.ThrowIfNull(formFile);

      if (formFile.Length == 0)
      {
        throw new ArgumentException(@"File submitted cannot be empty.", nameof(formFile));
      }

      if (formFile.Length > maxFileSize)
      {
        var megabyteMaxFileSize = maxFileSize / 1048576;
        throw new ArgumentException($"File provided exceeds {megabyteMaxFileSize:N1} MB.", nameof(formFile));
      }

      var trustedFileNameForDisplay = WebUtility.HtmlEncode(formFile.FileName);

      if (string.IsNullOrWhiteSpace(trustedFileNameForDisplay))
      {
        throw new ArgumentException(@"Uploaded file has an invalid or null name", nameof(formFile));
      }

      if (allowedFileExtensions.Length > 0)
      {
        var fileExt = Path.GetExtension(trustedFileNameForDisplay);

        if (!allowedFileExtensions.Contains(fileExt, StringComparer.OrdinalIgnoreCase))
        {
          throw new ArgumentException(@"File extension is not valid.", nameof(formFile));
        }
      }

      try
      {
        using (var fileStream = formFile.OpenReadStream())
        {
          var buffer = new byte[8];

          // This ensures the file has at least 8 bytes of data (i.e. more than a BOM).
          // It's more expensive, so we only want to do it on files that are about to be saved.
          if (await fileStream.ReadAsync(buffer.AsMemory(0, 8)) != 8)
          {
            throw new ArgumentException(@"The provided file is empty", nameof(formFile));
          }
        }
      }
      catch (Exception)
      {
        throw;

      }

      return true;
    }
  }
}
