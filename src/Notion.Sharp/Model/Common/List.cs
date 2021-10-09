using System;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record List<TItem>
    {
        public TItem[] Results { get; set; }

        [JsonPropertyName("next_cursor")]
        public Guid? NextCursor { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }
    }
}