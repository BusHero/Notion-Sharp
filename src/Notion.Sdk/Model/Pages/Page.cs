using System;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record Page
    {
        public Guid Id { get; set; }

        [JsonPropertyName("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonPropertyName("last_edited_time")]
        public DateTimeOffset LastEditedTime { get; set; }
        public bool Archived { get; set; }
        public File Icon { get; set; }
        public File Cover { get; set; }
        public IImmutableDictionary<string, PropertyValue> Properties { get; set; }
        public Parent Parent { get; set; }
        public Uri Url { get; set; }
    }
}