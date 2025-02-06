using ConfigChecker.DTOs;
using ConfigChecker.Services;
using Scalar.AspNetCore;
using System.Threading.Channels;

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

      // Use a channel for async communication between app sections.
      builder.Services.AddSingleton(Channel.CreateUnbounded<ProcessingRequest>());

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      app.UseHttpsRedirection();

      app.UseAntiforgery();
      app.UseAuthorization();

      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.MapScalarApiReference();
      }

      // Routes
      _ = app.MapGet("/", () => Results.Redirect("/upload"));

      _ = app.MapGet("/upload", () => Results.Ok());

      _ = app.MapPost("/upload", async (IFormFile file, IFileUploadService uploadService, ChannelWriter<ProcessingRequest> processingChannel) =>
      {
        string path = string.Empty;
        var reportId = Guid.NewGuid().ToString();

        path = await uploadService.ReadFormFile(file);

        if (path == string.Empty)
        {
          return Results.BadRequest("Unable to process provided file.");
        }

        while (await processingChannel.WaitToWriteAsync())
        {
          if (processingChannel.TryWrite(new ProcessingRequest { Path = path, ReportId = reportId }))
          {
            return Results.Accepted(uri: $"/reports/{reportId}");
          }
        }

        // Fell through, so an error occurred.
        return Results.BadRequest();
      });

      _ = app.MapGet("/reports/{reportId}", async (string reportId, IReportStore reportStore) =>
      {
        // Validate the provided ID is a GUID.
        if (!Guid.TryParse(reportId, out _))
        {
          return Results.BadRequest("Report ID was invalid");
        }

        var report = await reportStore.GetReport(reportId);

        if (report.Count > 0)
        {
          return Results.Ok(report);
        }

        return Results.NoContent();
      });

      app.Run();
    }
  }
}
