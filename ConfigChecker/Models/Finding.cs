namespace ConfigChecker.Models
{
  public sealed class Finding : EntityBase
  {
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
      ResourceName = string.Empty;
      Name = string.Empty;
      Description = string.Empty;
      Severity = SeverityRating.Invalid;
      CweId = string.Empty;
    }

    private Finding(string resourceName, string name, string description, SeverityRating severity, string cweId)
    {
      ResourceName = resourceName;
      Name = name;
      Description = description;
      Severity = severity;
      CweId = cweId;
    }

    /// <summary>
    /// Validates the inputs and creates a new <see cref="Finding"/> instance
    /// </summary>
    /// <param name="resourceName">Name of the resource where the weakness was found.</param>
    /// <param name="name">Short-name for the class of weakness.</param>
    /// <param name="description">Description of the weakness.</param>
    /// <param name="severity">The severity of the weakness.</param>
    /// <param name="cweId">The Common Weakness Enumeration ID of the weakness.</param>
    /// <returns>A new <see cref="Finding"/> instance.</returns>
    /// <exception cref="AggregateException">Contains validation errors.</exception>
    public static Finding Create(string resourceName, string name, string description, string severity, string cweId)
    {
      ValidateInputs(resourceName, name, description, severity, cweId);

      var parsedSeverity = Enum.Parse<SeverityRating>(severity, ignoreCase: true);
      
      return new Finding(resourceName, name, description, parsedSeverity, cweId);
    }

    private static void ValidateInputs(string resourceName, string name, string description, string severity, string cweId)
    {
      List<Exception> exceptions = [];

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

      if (string.IsNullOrEmpty(severity) || !Enum.TryParse<SeverityRating>(severity, ignoreCase: true, out _))
      {
        exceptions.Add(new ArgumentException("Cannot parse severity.", nameof(severity)));
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
