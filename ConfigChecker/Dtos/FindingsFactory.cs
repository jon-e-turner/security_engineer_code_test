using ConfigChecker.Models;
using static ConfigChecker.Dtos.FindingName;

namespace ConfigChecker.Dtos
{
  public static class FindingsFactory
  {
    // Slightly strange, but switch expressions are compiled into constant hashtables
    // making them the "right" way to create a constant dictionary at compile-time.
    private static string GetFindingDetails(FindingName findingName) => findingName switch
    {
      OpenRcePort => $@"Port {{0}} can be used to remotely administer a host, and should never be exposed to the public.",
      OpenPort => $@"Port {{0}} should not be exposed to the public without reason (e.g. web servers).",
      WeakPassword => $@"Password set in configuration does not meet minimum requirements.",
      _ => string.Empty
    };

    public static FindingDto CreateExposedPortFinding(this ResourceDto resource, int port)
    {
      return new FindingDto(
        resource.Name,
        OpenPort,
        string.Format(GetFindingDetails(OpenPort), port),
        @"Use network groups and secuirty admin rules to block access to the port.",
        FindingSeverity.High,
        "862"
        );
    }

    public static FindingDto CreateExposedRcePortFinding(this ResourceDto resource, int port)
    {
      return new FindingDto(
        resource.Name,
        OpenRcePort,
        string.Format(GetFindingDetails(OpenRcePort), port),
        @"Use network groups and secuirty admin rules to block access to the port.",
        FindingSeverity.Critical,
        "862"
        );
    }

    public static FindingDto CreateWeakPasswordFinding(this ResourceDto resource)
    {
      return new FindingDto(
        resource.Name,
        WeakPassword,
        GetFindingDetails(WeakPassword),
        @"Ensure password meets all requirements for length and complexity.",
        FindingSeverity.Critical,
        "1391"
        );
    }
  }
}
