using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Notion.Model;

public record Page : PageOrDatabase
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("created_time")]
    public DateTimeOffset CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTimeOffset LastEditedTime { get; set; }
    [JsonPropertyName("archived")]
    public bool Archived { get; set; }
    [JsonPropertyName("icon")]
    public Icon Icon { get; set; }
    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }
    [JsonPropertyName("properties")]
    public Dictionary<string, PropertyValue> Properties { get; set; }
    [JsonPropertyName("parent")]
    public Parent Parent { get; set; }
    [JsonPropertyName("url")]
    public Uri Url { get; set; }
}