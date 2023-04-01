using System.Text.Json.Serialization;

namespace Notion.Model;

public record Annotations
{
    [JsonPropertyName("bold")]
    public bool Bold { get; set; }
    [JsonPropertyName("italic")]
    public bool Italic { get; set; }
    [JsonPropertyName("strikethrough")]
    public bool Strikethrough { get; set; }
    [JsonPropertyName("underline")]
    public bool Underline { get; set; }
    [JsonPropertyName("code")]
    public bool Code { get; set; }
    [JsonPropertyName("color")]
    public Color Color { get; set; }
}
