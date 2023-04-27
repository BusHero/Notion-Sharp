using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public record Option
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [JsonPropertyName("id")] public string? Id { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [JsonPropertyName("name")] public string? Name { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [JsonPropertyName("color")] public string? Color { get; init; }
}
