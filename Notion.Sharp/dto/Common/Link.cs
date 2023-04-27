using System;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public record Link
{
    [JsonPropertyName("url")]
    public Uri? Url { get; init; }
}
