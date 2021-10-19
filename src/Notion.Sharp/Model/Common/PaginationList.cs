using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Notion.Model
{
    public record PaginationList<TItem> : IReadOnlyList<TItem>
    {
        [JsonPropertyName("object")]
        public const string Object = "list";

        [JsonPropertyName("results")]
        public TItem[] Results { get; set; }

        [JsonPropertyName("next_cursor")]
        public Guid? NextCursor { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        /// <inheritdoc/>
        public TItem this[int index] => ((IReadOnlyList<TItem>)Results)[index];

        /// <inheritdoc/>
        public int Count => ((IReadOnlyCollection<TItem>)Results).Count;

        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator() => ((IEnumerable<TItem>)Results).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();
    }
}