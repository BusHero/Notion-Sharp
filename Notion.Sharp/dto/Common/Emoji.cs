using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public class Emoji
{
    [JsonPropertyName("emoji")]
    public string? Value { get; init; }
}
