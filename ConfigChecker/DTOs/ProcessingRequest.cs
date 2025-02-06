namespace ConfigChecker.DTOs
{
  public record ProcessingRequest
  {
    public required string Path { get; set; }

    public required string ReportId {  get; set; }
  }
}
