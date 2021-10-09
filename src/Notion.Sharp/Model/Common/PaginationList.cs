using System;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record PaginationList<TItem>
    {
        [JsonPropertyName("object")]
        public const string Object = "list";

        [JsonPropertyName("results")]
        public TItem[] Results { get; set; }

        [JsonPropertyName("next_cursor")]
        public Guid? NextCursor { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }
    }
}