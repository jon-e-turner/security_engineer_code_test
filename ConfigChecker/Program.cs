using ConfigChecker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConfigChecker
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddAuthorization();
      builder.Services.AddScoped<IFileUploadService, FileUploadService>();

      var app = builder.Build();

      // Configure the HTTP request pipeline.

      app.UseHttpsRedirection();
      app.UseAntiforgery();
      app.UseAuthorization();

      _ = app.MapPost("/upload", async (IFormFile file, HttpRequest request) =>
      {
        var requestId = request.Headers.RequestId;

        using (var scope = app.Services.CreateScope())
        {
          var uploadService = scope.ServiceProvider.GetRequiredService<IFileUploadService>();

          var path = await uploadService.ReadFormFile(file);

          if (path == string.Empty)
          {
            return Results.BadRequest();
          }

          return Results.Accepted(requestId);
        }
      });

      _ = app.MapGet("/", () => "Hello world!")
          .Produces(200, typeof(string));

      app.Run();
    }
  }
}
