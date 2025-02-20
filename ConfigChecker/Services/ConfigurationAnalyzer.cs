using ConfigChecker.Abstractions;
using ConfigChecker.Dtos;
using ConfigChecker.Models;
using ConfigChecker.Utilities;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Channels;

namespace ConfigChecker.Services
{
  public class ConfigurationAnalyzer(
    IServiceScopeFactory scopeFactory,
    ChannelReader<ProcessingRequestDto> reader) : BackgroundService
  {
    private readonly ChannelReader<ProcessingRequestDto> _channelReader = reader;

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
      while (_channelReader is not null
             && !_channelReader.Completion.IsCompleted
             && await _channelReader.WaitToReadAsync(cancellation))
      {
        if (_channelReader.TryRead(out var request))
        {
          await AnalyzeConfigurationAsync(request.ReportId, request.Path, cancellation);
        }
      }
    }
    
    private async ValueTask AnalyzeConfigurationAsync(string reportId, string filePath, CancellationToken cancellation)
    {
      // Write as batch and read individually, can use a standard collection.
      List<ResourceDto> resourceRecords = [];

      // Write concurrently from many threads, so need to use a thread-safe collection.
      ConcurrentBag<FindingDto> findingRecords = [];

      using (var fileStream = File.OpenRead(filePath))
      {
        resourceRecords = await JsonSerializer.DeserializeAsync<List<ResourceDto>>(fileStream, cancellationToken: cancellation) ?? [];
      }

      if (resourceRecords.Count == 0)
      {
        return;
      }

      // Process the resource records and generate findings.
      while (!cancellation.IsCancellationRequested)
      {
        foreach (var res in resourceRecords)
        {
          switch (res.Type)
          {
            //case "virtual_machine":
            //  await foreach (var finding in AnalyzeVirtualMachineConfigurationAsync(res, cancellation))
            //  {
            //    findingRecords.Add(finding);
            //  }
            //  break;
            //case "storage_account":
            //  await foreach (var finding in AnalyzeStorageAccountConfigurationAsync(res, cancellation))
            //  {
            //    findingRecords.Add(finding);
            //  }
            //  break;
            case "database":
              await foreach (var finding in AnalyzeDatabaseConfigurationAsync(res, cancellation))
              {
                findingRecords.Add(finding);
              }
              break;
            default:
              // Unknown resource type. Log and drop silently.
              break;
          }
        }
      }

      // Persist findings to datastore.
      await TryPersistToDatastore(reportId, [.. findingRecords], cancellation);
    }

    private static async IAsyncEnumerable<FindingDto> AnalyzeDatabaseConfigurationAsync(
      ResourceDto res, 
      [EnumeratorCancellation] CancellationToken cancellation)
    {
      if (res is not null && res.Type.Equals("database") && res.SecuritySettings is not null)
      {
        if (res.SecuritySettings.TryGetValue(@"open_ports", out var portJson))
        {
          await foreach (var f in FindingsUtilities.GetOpenPortFindings(res, portJson))
          {
            yield return f;
          }
        }
      }
    }
    
    //private static async IAsyncEnumerable<FindingDto> AnalyzeStorageAccountConfigurationAsync(
    //  ResourceDto res,
    //  [EnumeratorCancellation] CancellationToken cancellation)
    //{
    //  throw new NotImplementedException();
    //}

    //private static async IAsyncEnumerable<FindingDto> AnalyzeVirtualMachineConfigurationAsync(
    //  ResourceDto res,
    //  [EnumeratorCancellation] CancellationToken cancellation)
    //{
    //  throw new NotImplementedException();
    //}

    private async ValueTask TryPersistToDatastore(string reportId, List<FindingDto> findings, CancellationToken cancellation)
    {
      while (!cancellation.IsCancellationRequested)
      {
        using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        IReportStore reportStore = scope.ServiceProvider.GetRequiredService<IReportStore>();

        // To-Do: wrap this in a try block and capture validation errors.
        List<Finding> findingEntities = [.. findings.Select(f => Finding.Create(reportId, f))];

        await reportStore.AppendToReportAsync(findingEntities);
      }
    }
  }
}
