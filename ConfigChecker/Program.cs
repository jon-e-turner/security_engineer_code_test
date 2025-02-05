using ConfigChecker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Scalar.AspNetCore;

namespace ConfigChecker
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddAuthorization();

      builder.Services.AddOptions<FileUploadServiceOptions>()
        .BindConfiguration(nameof(FileUploadServiceOptions));

      builder.Services.AddSingleton<IConfigurationAnalyzer, ConfigurationAnalyzer>();
      builder.Services.AddScoped<IFileUploadService, FileUploadService>();

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      app.UseHttpsRedirection();

      app.UseAntiforgery();
      app.UseAuthorization();
      
      if(app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.MapScalarApiReference();
      }

      _ = app.MapGet("/", () => Results.Redirect("/upload"));

      _ = app.MapGet("/upload", () => Results.Ok());
      
      _ = app.MapPost("/upload", async (IFormFile file) =>
      {
        string path = string.Empty;
        var reportId = Guid.NewGuid().ToString();

        using (var scope = app.Services.CreateAsyncScope())
        {
          var uploadService = scope.ServiceProvider.GetRequiredService<IFileUploadService>();

          path = await uploadService.ReadFormFile(file);

          if (path == string.Empty)
          {
            return Results.BadRequest();
          }
        }

        var configAnalyzer = app.Services.GetRequiredService<IConfigurationAnalyzer>();
        await configAnalyzer.AnalyzeConfiguration(reportId, path);

        return Results.Accepted(uri: $"/reports/{reportId}");
      });

      _ = app.MapGet("/reports/{reportId}", async (string reportId) => 
      {
        using (var scope = app.Services.CreateAsyncScope())
        {
          var reportContext = scope.ServiceProvider.GetRequiredService<IReportStore>();
        }

        return Results.BadRequest();
      });

      app.Run();
    }
  }
}
