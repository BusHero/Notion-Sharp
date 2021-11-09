using System.Text.Json.Serialization;

namespace Notion.Model;

public class Emoji
{
    [JsonPropertyName("emoji")]
    public string Value { get; init; }
}
