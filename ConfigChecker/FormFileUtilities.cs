using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ConfigChecker
{
  public static class FormFileUtilities
  {
    public static async Task<IFormFile> ValidateFormFile(IFormFile? formFile, long maxFileSize, string[] allowedFileExtensions)
    {
      var modelSate = new ModelStateDictionary();

      var temp = await ValidateFormFile(formFile, modelSate, maxFileSize, allowedFileExtensions);

      if (!modelSate.IsValid || temp is null)
      {
        List<Exception> aggregateExceptions = [];

        foreach (var error in modelSate.Values.SelectMany(v => v.Errors))
        {
          aggregateExceptions.Add(new ArgumentException(error.ErrorMessage));
        }

        throw new AggregateException(aggregateExceptions);
      }

      return temp;
    }

    public static async Task<IFormFile?> ValidateFormFile(IFormFile? formFile, ModelStateDictionary modelState,
                                                          long maxFileSize, string[] allowedFileExtensions)
    {
      // Check the file length. This check doesn't catch files that only have 
      // a BOM as their content.
      if (formFile is null || formFile.Length == 0)
      {
        modelState.AddModelError(formFile?.Name ?? @"nullCheck", @"The provided file is empty.");

        return null;
      }

      if (formFile.Length > maxFileSize)
      {
        var megabyteMaxFileSize = maxFileSize / 1048576;
        modelState.AddModelError(formFile.Name, $"File provided exceeds {megabyteMaxFileSize:N1} MB.");

        return null;
      }

      var trustedFileNameForDisplay = WebUtility.HtmlEncode(formFile.FileName);

      if (string.IsNullOrWhiteSpace(trustedFileNameForDisplay))
      {
        modelState.AddModelError(formFile.Name, @"Uploaded file has an invalid or null name");

        return null;
      }

      if (allowedFileExtensions.Length > 0)
      {
        var fileExt = Path.GetExtension(trustedFileNameForDisplay);

        if (!allowedFileExtensions.Contains(fileExt, StringComparer.OrdinalIgnoreCase))
        {
          modelState.AddModelError(formFile.Name, @"File extension is not valid.");

          return null;
        }
      }

      try
      {
        using (var fileStream = formFile.OpenReadStream())
        {
          var buffer = new Byte[8];

          // This ensures the file has at least 8 bytes of data (i.e. more than a BOM).
          // It's more expensive, so we only want to do it on files that are about to be saved.
          if (await fileStream.ReadAsync(buffer.AsMemory(0, 8)) != 8)
          {
            modelState.AddModelError(formFile.Name, @"The provided file is empty");

            return null;
          }
        }
      }
      catch (Exception ex)
      {
        modelState.AddModelError(formFile.Name, ex.ToString());
      }

      return formFile;
    }
  }
}
