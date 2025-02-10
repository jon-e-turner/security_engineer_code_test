using ConfigChecker.DTOs;
using ConfigChecker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ConfigChecker.Endpoints
{
  /// <summary>
  /// Extension methods for <see cref="IEndpointRouteBuilder"/> to map our endpoints.
  /// </summary>
  public static class ConfigCheckerEndpoints
  {
    public static void MapConfigCheckerEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
      _ = endpointRouteBuilder.MapPost("/upload", async (IFormFile file, IFileUploadService uploadService, [FromServices] ChannelWriter<ProcessingRequestDto> processingChannel) =>
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
          if (processingChannel.TryWrite(new ProcessingRequestDto(path, reportId)))
          {
            return Results.Accepted(uri: $"/reports/{reportId}");
          }
        }

        // Fell through, so an error occurred.
        return Results.BadRequest();
      });

      _ = endpointRouteBuilder.MapGet("/reports/{reportId}", async (string reportId, [FromServices] IReportStore reportStore) =>
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
    }
  }
}
