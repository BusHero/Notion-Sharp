using System;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public record File
{
    public RichText[]? Caption { get; init; }

    public string? Name { get; init; }

    public record External : File
    {
        public Uri? Uri { get; init; }
    }

    public record Internal : File
    {
        public Uri? Uri { get; set; }
        public DateTime ExpireTime { get; set; }
    }

    public record Emoji : File
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? Value { get; set; }
    }
    
    public T Copy<T>() where T : File, new() => new()
    {
        Name = Name,
        Caption = Caption,
    };
}
