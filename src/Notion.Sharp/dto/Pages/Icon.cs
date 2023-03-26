using System;

namespace Notion.Model;

public record Icon
{
    public record Emoji: Icon
    {
        public string? Value { get; set; }
    }

    public record External: Icon
    {
        public Uri? Url { get; set; }
    }

    public record File: Icon
    {
        public Uri? Url { get; set; }
        public DateTimeOffset ExpiryTime { get; set; }
    }
}