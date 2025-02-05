using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigChecker.Models
{
  public class Resource : EntityBase
  {
    public string Type { get; private set; }

    public string Name { get; private set; }

    public IDictionary<string, string> AzureSpecific { get; private set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? SecuritySettings { get; private set; }
  }
}
