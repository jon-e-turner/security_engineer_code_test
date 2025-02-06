using ConfigChecker.DTOs;
using ConfigChecker.Persistance;
using ConfigChecker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
      builder.Services.AddAntiforgery();

      builder.Services.AddOptions<FileUploadServiceOptions>()
        .BindConfiguration(nameof(FileUploadServiceOptions));

      // Add worker services.
      //builder.Services.AddSingleton<IConfigurationAnalyzer, ConfigurationAnalyzer>();
      builder.Services.AddScoped<IFileUploadService, FileUploadService>();

      // Add persistence via DbContext.
      builder.Services.AddDbContext<FindingsDbContext>(options =>
      {
        var connectionString = builder.Configuration.GetConnectionString(nameof(FindingsDbContext))
          ?? throw new ArgumentException("Connection string not set in appSettings.json.");

        options.UseSqlite(connectionString);
      });

      // Use a channel for async communication between app sections.
      builder.Services.AddSingleton(Channel.CreateUnbounded<ProcessingRequestDto>());

      var app = builder.Build();

      app.UseAntiforgery();
      app.UseAuthorization();

      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHttpsRedirection();
      }

      // Routes
      _ = app.MapGet("/", () => Results.Redirect("/upload"));

      _ = app.MapGet("/upload", () => Results.Ok());

      _ = app.MapPost("/upload", async (IFormFile file, IFileUploadService uploadService, [FromServices] ChannelWriter<ProcessingRequestDto> processingChannel) =>
      {
        string path = string.Empty;
        var reportId = Guid.NewGuid().ToString();

        path = await uploadService.ReadFormFileAsync(file);

        if (path == string.Empty)
        {
          return Results.BadRequest("Unable to process provided file.");
        }

        while (await processingChannel.WaitToWriteAsync())
        {
          if (processingChannel.TryWrite(new ProcessingRequestDto(path,reportId)))
          {
            return Results.Accepted(uri: $"/reports/{reportId}");
          }
        }

        // Fell through, so an error occurred.
        return Results.BadRequest();
      });

      _ = app.MapGet("/reports/{reportId}", async (string reportId, [FromServices] IReportStore reportStore) =>
      {
        // Validate the provided ID is a GUID.
        if (!Guid.TryParse(reportId, out _))
        {
          return Results.BadRequest("Report ID was invalid");
        }

        List<FindingDto> report = [];

        await foreach (FindingDto f in reportStore.GetReportAsync(reportId))
        {
          report.Add(f);
        }

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
