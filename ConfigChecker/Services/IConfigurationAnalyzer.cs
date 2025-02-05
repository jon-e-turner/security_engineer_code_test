namespace ConfigChecker.Services
{
  public interface IConfigurationAnalyzer
  {
    public ValueTask AnalyzeConfiguration(string requestId, string filePath);
  }
}
