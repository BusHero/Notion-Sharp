using System;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record NavigationPage<TItem>
    {
        public string Object => "list";
        public TItem[] Results { get; set; }
        [JsonPropertyName("next_cursor")]
        public Guid? NextCursor { get; set; }
        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }
    }
}