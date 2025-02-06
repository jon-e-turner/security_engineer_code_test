using ConfigChecker.DTOs;
using System.Text.Json;
using System.Threading.Channels;

namespace ConfigChecker.Services
{
  public class ConfigurationAnalyzer(ChannelReader<ProcessingRequestDto> reader) : BackgroundService, IConfigurationAnalyzer
  {
    private readonly ChannelReader<ProcessingRequestDto> _channelReader = reader;

    public async ValueTask AnalyzeConfigurationAsync(string reportId, string filePath)
    {
      List<ResourceDto> resources = [];

      using (var fileStream = File.OpenRead(filePath))
      {
        resources = await JsonSerializer.DeserializeAsync<List<ResourceDto>>(fileStream) ?? [];
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
          await AnalyzeConfigurationAsync(request.ReportId, request.Path);
        }
      }
    }
  }
}
