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

      try
      {
        // Process the resource records and generate findings.
        while (!cancellation.IsCancellationRequested)
        {
          await Parallel.ForEachAsync(resourceRecords, async (res, cancellation) =>
          {
            await foreach (var finding in AnalyzeResourceConfigurationAsync(res, cancellation))
            {
              findingRecords.Add(finding);
            }
          });
        }
      }
      finally
      {
        // Persist findings to datastore.
        await TryPersistToDatastore(reportId, [.. findingRecords], cancellation);
      }
    }

    private static async IAsyncEnumerable<FindingDto> AnalyzeResourceConfigurationAsync(
      ResourceDto res,
      [EnumeratorCancellation] CancellationToken cancellation)
    {
      while (!cancellation.IsCancellationRequested)
      {
        if (res is not null && res.SecuritySettings is not null)
        {
          if (res.SecuritySettings.TryGetValue(@"open_ports", out var portJson))
          {
            await foreach (var f in res.GetOpenPortFindings(portJson))
            {
              yield return f;
            }
          }

          if (res.SecuritySettings.TryGetValue(@"password", out var pwdJson))
          {
            var finding = await res.GetWeakPasswordFinding(pwdJson);

            if (finding is not null)
            {
              yield return finding;
            }

            yield break;
          }

          if (res.SecuritySettings.TryGetValue(@"mfa_enabled", out var mfaSetting))
          {
            var finding = await res.GetMfaDisabledFinding(mfaSetting);

            if (finding is not null)
            {
              yield return finding;
            }

            yield break;
          }
          else if (res.Type.Equals("database") || res.Type.Equals("virtual_machine"))
          {
            yield return res.GetMfaDisabledFinding();
          }

          if (res.SecuritySettings.TryGetValue(@"encryption", out var encryptSetting))
          {
            var finding = await res.GetEncryptionDisabledFinding(encryptSetting);

            if (finding is not null)
            {
              yield return finding;
            }

            yield break;
          }
        }
      }
    }

    private async ValueTask TryPersistToDatastore(string reportId, List<FindingDto> findings, CancellationToken cancellation)
    {
      while (!cancellation.IsCancellationRequested)
      {
        List<Finding> findingEntities = [];
        using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        IReportStore reportStore = scope.ServiceProvider.GetRequiredService<IReportStore>();

        try
        {
          findingEntities = [.. findings.Select(f => Finding.Create(reportId, f))];
        }
        //catch (AggregateException)
        //{
        //  In production, log the errors, then move on.
        //}
        finally
        {
          await reportStore.AppendToReportAsync(findingEntities);
        }
      }
    }
  }
}
