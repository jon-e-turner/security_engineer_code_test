namespace ConfigChecker.Dtos
{
  public record FindingDto(
    string ResourceName,
    FindingName Name,
    string Description,
    string Mitigation,
    FindingSeverity Severity,
    string CweId
  );
}
