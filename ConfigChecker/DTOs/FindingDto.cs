namespace ConfigChecker.Dtos
{
  public record FindingDto(
    string ResourceName,
    string Name,
    string Description,
    string Mitigation,
    string Severity,
    string CweId
  );
}
