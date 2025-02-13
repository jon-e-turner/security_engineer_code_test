using ConfigChecker.DTOs;

namespace ConfigChecker.Models
{
  public sealed class Finding : EntityBase
  {
    /// <summary>
    /// The report this finding belongs to.
    /// </summary>
    public string ReportId { get; private set; }
    
    /// <summary>
    /// Name of the resource where the weakness was found.
    /// </summary>
    public string ResourceName { get; private set; }

    /// <summary>
    /// Short-name for the class of weakness.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Description of the weakness.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Recommended mitigation steps.
    /// </summary>
    public string Mitigation { get; private set; }

    /// <summary>
    /// The severity of the weakness.
    /// </summary>
    public SeverityRating Severity { get; private set; }

    /// <summary>
    /// The Common Weakness Enumeration ID of the weakness.
    /// CWE are the industry-standard for describing weaknesses.
    /// </summary>
    public string CweId { get; private set; }

    private Finding()
    {
      ReportId = string.Empty;
      ResourceName = string.Empty;
      Name = string.Empty;
      Description = string.Empty;
      Mitigation = string.Empty;
      Severity = SeverityRating.Invalid;
      CweId = string.Empty;
    }

    private Finding(string reportId, string resourceName, string name, string description, string mitigation, SeverityRating severity, string cweId)
    {
      ReportId = reportId;
      ResourceName = resourceName;
      Name = name;
      Description = description;
      Mitigation = mitigation;
      Severity = severity;
      CweId = cweId;
    }

    /// <summary>
    /// Validates the inputs and creates a new <see cref="Finding"/> instance.
    /// </summary>
    /// <param name="reportId">The report this finding belongs to.</param>
    /// <param name="resourceName">Name of the resource where the weakness was found.</param>
    /// <param name="name">Short-name for the class of weakness.</param>
    /// <param name="description">Description of the weakness.</param>
    /// <param name="mitigation">Recommended mitigation steps.</param>
    /// <param name="severity">The severity of the weakness.</param>
    /// <param name="cweId">The Common Weakness Enumeration ID of the weakness.</param>
    /// <returns>A new <see cref="Finding"/> instance.</returns>
    /// <exception cref="AggregateException">Contains validation errors.</exception>
    public static Finding Create(string reportId, string resourceName, string name, string description, string mitigation, string severity, string cweId)
    {
      ValidateInputs(reportId, resourceName, name, description, mitigation, severity, cweId);

      var parsedSeverity = Enum.Parse<SeverityRating>(severity, ignoreCase: true);
      
      return new Finding(reportId, resourceName, name, description, mitigation, parsedSeverity, cweId);
    }

    /// <summary>
    /// Validates the inputs and creates a new <see cref="Finding"/> instance.
    /// </summary>
    /// <param name="reportId">The report this finding belongs to.</param>
    /// <param name="findingDto">The DTO describing the finding.</param>
    /// <returns>A new <see cref="Finding"/> instance.</returns>
    /// <exception cref="AggregateException">Contains validation errors.</exception>
    public static Finding Create(string reportId, FindingDto findingDto)
    {
      return Finding.Create(reportId,
                            findingDto.ResourceName,
                            findingDto.Name,
                            findingDto.Description,
                            findingDto.Mitigation,
                            findingDto.Severity,
                            findingDto.CweId);
    }

    private static void ValidateInputs(string reportId, string resourceName, string name, string description, string mitigation, string severity, string cweId)
    {
      List<Exception> exceptions = [];

      if (string.IsNullOrEmpty(reportId) || !Guid.TryParse(reportId, out _))
      {
        exceptions.Add(new ArgumentException("Report ID is not valid.", nameof(reportId)));
      }

      if (string.IsNullOrWhiteSpace(resourceName))
      {
        exceptions.Add(new ArgumentException("Resource name cannot be empty.", nameof(resourceName)));
      }

      if (string.IsNullOrWhiteSpace(name))
      {
        exceptions.Add(new ArgumentException("Name cannot be empty.", nameof(name)));
      }

      if (string.IsNullOrEmpty(description))
      {
        exceptions.Add(new ArgumentException("Description cannot be empty.", nameof(description)));
      }

      if (string.IsNullOrEmpty(mitigation))
      {
        exceptions.Add(new ArgumentException("Mitigation recommendataion cannot be empty.", nameof(mitigation)));
      }

      if (string.IsNullOrEmpty(severity) || !Enum.TryParse<SeverityRating>(severity, ignoreCase: true, out _))
      {
        exceptions.Add(new ArgumentException("Cannot parse severity.", nameof(severity)));
      }

      if (string.IsNullOrEmpty(cweId))
      {
        exceptions.Add(new ArgumentException("CWE Id cannot be empty.", nameof(cweId)));
      }

      if (exceptions.Count > 0)
      {
        throw new AggregateException(exceptions);
      }
    }

    /// <summary>
    /// Standardized severity ratings.
    /// </summary>
    public enum SeverityRating
    {
      Invalid = 0,
      Informational,
      Low,
      Medium,
      High,
      Critical
    }
  }
}
