using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public record Database : PageOrDatabase
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("created_time")] public DateTimeOffset CreatedTime { get; set; }
    [JsonPropertyName("last_edited_time")] public DateTimeOffset LastEditedTime { get; set; }
    [JsonPropertyName("title")] public RichText[]? Title { get; init; }
    [JsonPropertyName("icon")] public File? Icon { get; init; }
    [JsonPropertyName("cover")] public File? Cover { get; init; }
    [JsonPropertyName("properties")] public Dictionary<string, Property>? Properties { get; init; }
    [JsonPropertyName("parent")] public Parent? Parent { get; init; }
    [JsonPropertyName("is_inline")] public bool IsInline { get; init; }
    [JsonPropertyName("archived")] public bool Archived { get; init; }
    [JsonPropertyName("url")] public Uri? Url { get; set; }
    [JsonPropertyName("created_by")] public User? CreatedBy { get; init; }
    [JsonPropertyName("last_edited_by")] public User? LastEditedBy { get; init; }
}
