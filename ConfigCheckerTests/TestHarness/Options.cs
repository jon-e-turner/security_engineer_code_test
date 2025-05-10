using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigCheckerTests.TestHarness
{
    internal static class Options
    {
        internal static JsonSerializerOptions SerializerOptions
        {
            get;
        } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
        };
    }
}
