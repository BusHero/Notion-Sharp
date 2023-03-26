namespace Notion.Sharp.Tests.Utils;

internal static class JsonExtensions
{
    internal static string Formatted(this string json)
    {
        var document = JsonDocument.Parse(json);
        var jsonSerialized = JsonSerializer.Serialize(
            document, 
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
        return jsonSerialized;
    }
}