using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigChecker.Dtos
{
  public record ResourceDto
  {
    public required string Type { get; set; }

    public required string Name { get; set; }

    [JsonPropertyName("azure_specific")]
    public required IDictionary<string, string> AzureSpecific { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? SecuritySettings { get; set; }
  }
}
