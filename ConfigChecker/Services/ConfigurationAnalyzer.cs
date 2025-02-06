using ConfigChecker.DTOs;
using System.Text.Json;
using System.Threading.Channels;

namespace ConfigChecker.Services
{
  public class ConfigurationAnalyzer(ChannelReader<ProcessingRequest> reader) : BackgroundService, IConfigurationAnalyzer
  {
    private readonly ChannelReader<ProcessingRequest> _channelReader = reader;

    public async ValueTask AnalyzeConfiguration(string reportId, string filePath)
    {
      List<Resource> resources = [];

      using (var fileStream = File.OpenRead(filePath))
      {
        resources = await JsonSerializer.DeserializeAsync<List<Resource>>(fileStream) ?? [];
      }

      if (resources.Count == 0)
      {
        return;
      }


    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (_channelReader != null && !_channelReader.Completion.IsCompleted && await _channelReader.WaitToReadAsync(stoppingToken))
      {
        if (_channelReader.TryRead(out var request))
        {
          await AnalyzeConfiguration(request.ReportId, request.Path);
        }
      }
    }
  }
}
