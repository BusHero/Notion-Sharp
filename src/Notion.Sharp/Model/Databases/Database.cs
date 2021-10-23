using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Notion.Model;

public record Database : PageOrDatabase
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("created_time")]
    public DateTimeOffset CreatedTime { get; set; }
    [JsonPropertyName("last_edited_time")]
    public DateTimeOffset LastEditedTime { get; set; }
    [JsonPropertyName("title")]
    public RichText[] Title { get; set; }
    [JsonPropertyName("icon")]
    public File Icon { get; set; }
    [JsonPropertyName("cover")]
    public File Cover { get; set; }
    [JsonPropertyName("properties")]
    public Dictionary<string, Property> Properties { get; set; }
    [JsonPropertyName("parent")]
    public Parent Parent { get; set; }
}
