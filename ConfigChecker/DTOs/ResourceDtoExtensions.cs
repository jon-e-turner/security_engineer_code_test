using ConfigChecker.Models;

namespace ConfigChecker.DTOs
{
  public static class ResourceDtoExtensions
  {
    // Slightly strange, but switch expressions are compiled into constant hashtables
    // making them the "right" way to create a constant dictionary at compile-time.
    // Finding names should probably go into an enumerable.
    private static string GetFindingDetails(string findingName) => findingName switch
    {
      "OpenRcePort" => $@"Port {{0}} can be used to remotely administer a host, and should never be exposed to the public.",
      "OpenPort" => $@"Port {{0}} should not be exposed to the public without reason (e.g. web servers).",
      _ => string.Empty
    };

    public static FindingDto CreateExposedPortFinding(this ResourceDto resource, int port)
    {
      return new FindingDto(
        resource.Name,
        "OpenPort",
        string.Format(GetFindingDetails("OpenPort"), port),
        @"Use network groups and secuirty admin rules to block access to the port.",
        Finding.SeverityRating.High.ToString(),
        "862"
        );
    }

    public static FindingDto CreateExposedRcePortFinding(this ResourceDto resource, int port)
    {
      return new FindingDto(
        resource.Name,
        "OpenRcePort",
        string.Format(GetFindingDetails("OpenRcePort"), port),
        @"Use network groups and secuirty admin rules to block access to the port.",
        Finding.SeverityRating.Critical.ToString(),
        "862"
        );
    }
  }
}
