namespace ConfigChecker.Abstractions
{
  public interface IConfigurationAnalyzer
  {
    public ValueTask AnalyzeConfigurationAsync(string requestId, string filePath);
  }
}
