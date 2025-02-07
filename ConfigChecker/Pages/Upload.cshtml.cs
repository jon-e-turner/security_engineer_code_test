using ConfigChecker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace ConfigChecker.Pages
{
  public class UploadModel : PageModel
  {
    private readonly long _maxFileSize;
    private readonly string[] _allowedExtensions = [];

    [BindProperty]
    public IFormFile? FormFile { get; set; }
    
    public string? Result { get; private set; }

    public UploadModel(IOptionsSnapshot<FileUploadServiceOptions> options)
    {
      _maxFileSize = options.Value.MaxFileSize;
      _allowedExtensions = options.Value.AllowedExtensions;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostUploadAsync()
    {
      if (!ModelState.IsValid)
      {
        Result = "File submitted was not valid.";

        return Page();
      }

      var validatedFile = FormFileUtilities.ValidateFormFile(FormFile, ModelState, _maxFileSize, _allowedExtensions);

      if (!ModelState.IsValid || validatedFile is null)
      {
        Result = "File submitted was not valid.";

        return Page();
      }

      // How do I call the API endpoint from here? Or does this replace the API endpoint?
      return Page();
    }
  }
}
