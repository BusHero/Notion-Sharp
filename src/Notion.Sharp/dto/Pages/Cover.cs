using System;

namespace Notion.Model;

public record Cover
{
    public record External: Cover
    {
        public Uri? Url { get; set; }
    }

    public record File: Cover
    {
        public Uri? Url { get; set; }
        public DateTimeOffset ExpiryTime { get; set; }
    }
}