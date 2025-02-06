using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigChecker.DTOs
{
  public record Resource
  {
    public required string Type { get; set; }

    public required string Name { get; set; }

    public required IDictionary<string, string> AzureSpecific { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? SecuritySettings { get; private set; }
  }
}
