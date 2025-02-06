namespace ConfigChecker.Services
{
  public interface IConfigurationAnalyzer
  {
    public ValueTask AnalyzeConfigurationAsync(string requestId, string filePath);
  }
}
