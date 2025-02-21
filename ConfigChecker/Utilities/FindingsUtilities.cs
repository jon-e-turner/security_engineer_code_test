using ConfigChecker.Dtos;
using System.Text.Json;

namespace ConfigChecker.Utilities
{
  public static class FindingsUtilities
  {
    public static async Task<FindingDto?> GetMfaDisabledFinding(this ResourceDto res, JsonElement mfaSetting)
    {
      try
      {
        if (await Task.Run(mfaSetting.GetBoolean))
        {
          return res.CreateMfaDisabledFinding();
        }
      }
      catch (Exception ex) when
        (ex is InvalidOperationException || ex is ObjectDisposedException)
      {
        // JSON could not be parsed, so log and move on.
      }

      return null;
    }

    public static FindingDto GetMfaDisabledFinding(this ResourceDto res)
    {
      return res.CreateMfaDisabledFinding();
    }

    public static async Task<FindingDto?> GetEncryptionDisabledFinding(this ResourceDto res, JsonElement encryptSetting)
    {
      try
      {
        if (await Task.Run(encryptSetting.GetBoolean))
        {
          return res.CreateEncryptionDisabledFinding();
        }
      }
      catch (Exception ex) when
        (ex is InvalidOperationException || ex is ObjectDisposedException)
      {
        // JSON could not be parsed, so log and move on.
      }

      return null;
    }

    public static async Task<FindingDto?> GetWeakPasswordFinding(this ResourceDto res, JsonElement pwdJson)
    {
      try
      {
        string password = await Task.Run(() => pwdJson.Deserialize<string>() ?? string.Empty);

        // Use a real algorithm in production, obvs.
        if (string.IsNullOrEmpty(password) || password.Contains("weak"))
        {
          return res.CreateWeakPasswordFinding();
        }
      }
      catch (Exception ex) when
        (ex is JsonException || ex is NotSupportedException)
      {
        // JSON could not be parsed, so log and move on.
      }

      return null;
    }

    public static async IAsyncEnumerable<FindingDto> GetOpenPortFindings(this ResourceDto res, JsonElement portJson)
    {
      int[] ports;

      try
      {
        ports = await Task.Run(() => portJson.Deserialize<int[]>() ?? []);
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

    private static IEnumerable<FindingDto> GetOpenPortFindings(this ResourceDto res, int[] ports, int[]? allowedPorts = null)
    {
      var portsToCheck = ports.Except(allowedPorts ?? []);

      foreach (var port in portsToCheck)
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
