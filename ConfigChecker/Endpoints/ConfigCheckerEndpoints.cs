using ConfigChecker.Abstractions;
using ConfigChecker.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
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
      _ = endpointRouteBuilder.MapGet("/healthcheck", () =>
            TypedResults.NoContent());

      _ = endpointRouteBuilder.MapPost("/upload",
        async Task<Results<Accepted, BadRequest<string>, BadRequest>> (
          [FromForm] IFormFile file,
          [FromServices] IFileUploadService uploadService,
          [FromServices] ChannelWriter<ProcessingRequestDto> processingChannel) =>
      {
        string path = string.Empty;
        var reportId = Guid.NewGuid().ToString();

        path = await uploadService.ReadFormFileAsync(file);

        if (path == string.Empty)
        {
          return TypedResults.BadRequest("Unable to process provided file.");
        }

        while (await processingChannel.WaitToWriteAsync())
        {
          if (processingChannel.TryWrite(new ProcessingRequestDto(path, reportId)))
          {
            return TypedResults.Accepted(uri: $"/reports/{reportId}");
          }
        }

        // Fell through, so an error occurred.
        return TypedResults.BadRequest();
      });

      _ = endpointRouteBuilder.MapGet("/reports/{reportId}",
        async Task<Results<Ok<List<FindingDto>>, NoContent, BadRequest<string>>> (
          [FromRoute] string reportId,
          [FromServices] IReportStore reportStore) =>
      {
        // Validate the provided ID is a GUID.
        if (!Guid.TryParse(reportId, out _))
        {
          return TypedResults.BadRequest("Report ID was invalid");
        }

        List<FindingDto> report = [];

        await foreach (FindingDto f in reportStore.GetReportAsync(reportId))
        {
          report.Add(f);
        }

        if (report.Count > 0)
        {
          return TypedResults.Ok(report);
        }

        return TypedResults.NoContent();
      });
    }
  }
}
