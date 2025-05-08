using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigCheckerBlazor.Models
{
    public record ResourceDto
    {
        public required string Type
        {
            get;
            init;
        }

        public required string Name
        {
            get;
            init;
        }

        [ JsonPropertyName("azure_specific") ]
        public required IDictionary<string, string> AzureSpecific
        {
            get;
            init;
        }

        [ JsonExtensionData ]
        public IDictionary<string, JsonElement>? SecuritySettings
        {
            get;
            init;
        }
    }
}
