using System;
using System.Text.Json.Serialization;

namespace Notion.Model;

public record Link
{
    [JsonPropertyName("url")]
    public Uri Url { get; init; }
}
