namespace ConfigChecker.DTOs
{
  public record FindingsDto(
    string ResourceName,
    string Name,
    string Description,
    string Severity,
    string CweId
  );
}
