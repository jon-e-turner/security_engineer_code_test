using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigCheckerTests.TestHarness
{
    internal static class Options
    {
        private readonly static JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
            Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
                }
        };

        internal static JsonSerializerOptions SerializerOptions => _options;
    }
}
