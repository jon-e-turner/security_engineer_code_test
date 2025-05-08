namespace ConfigCheckerBlazor.Models
{
    public abstract record FindingDto(
        string ResourceName,
        FindingName Name,
        string Description,
        string Mitigation,
        FindingSeverity Severity,
        string CweId
    );
}
