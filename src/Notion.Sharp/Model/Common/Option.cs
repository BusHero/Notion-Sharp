using System;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record Option
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("color")]
        public string Color { get; init; }
    }
}