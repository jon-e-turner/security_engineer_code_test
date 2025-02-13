using ConfigChecker.DTOs;
using System.Text.Json;

namespace ConfigChecker.Utilities
{
  public static class FindingsUtilities
  {
    public static async IAsyncEnumerable<FindingDto> GetOpenPortFindings(ResourceDto res, JsonElement portJson)
    {
      int[] ports = [];

      try
      {
        ports = portJson.Deserialize<int[]>(JsonSerializerOptions.Default) ?? [];
      }
      catch (Exception ex) when
        (ex is JsonException || ex is NotSupportedException)
      {
        // JSON could not be parsed, so log and move on.
        yield break;
      }

      foreach (var f in GetOpenPortFindings(res, ports))
      {
        yield return f;
      }
    }

    private static IEnumerable<FindingDto> GetOpenPortFindings(ResourceDto res, int[] ports)
    {
      foreach (var port in ports)
      {
        var finding = port switch
        {
          22 or
            3389 or
            5985 or
            5986 => res.CreateExposedRcePortFinding(port),
          80 or
            443 or
            8080 => res.CreateExposedPortFinding(port),
          _ => null
        };

        if (finding is not null)
        {
          yield return finding;
        }

        yield break;
      }
    }
  }
}
