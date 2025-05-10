namespace ConfigChecker.Abstractions.Dtos
{
    /// <summary>
    ///     Standardized severity ratings.
    /// </summary>
    public enum FindingSeverity
    {
        Invalid = 0,
        Informational,
        Low,
        Medium,
        High,
        Critical
    }
}
