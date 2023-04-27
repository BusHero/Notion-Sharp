using System;
using Notion.Model;

namespace Notion.experiment;

public record PropertyItem
{
    public string? Object { get; set; }
    public string? Type { get; set; }
    public RichText2? Title { get; set; }
    public People1? People { get; set; }
    public People1? RichText { get; set; }
    public Relation1? Relation { get; set; }
}

public record RichText2
{
    public string? Type { get; set; }
    public Text? Text { get; set; }
    public Annotations? Annotations { get; set; }
    public string? PlainText { get; set; }
    public object? Href { get; set; }
}

public record Text
{
    public string? Content { get; set; }
    public object? Link { get; set; }
}

public record Foo
{
    public string? Object { get; set; }
    public PropertyItem[]? Results { get; set; }
    public object? NextCursor { get; set; }
    public bool HasMore { get; set; }
    public string? Type { get; set; }
    public RichText2? Title { get; set; }
    public DateTime? CreatedTime { get; set; }
    public Date1? Date { get; set; }
    public People1? LastEditedBy { get; set; }
    public People1? CreatedBy { get; set; }
    public DateTime? LastEditedTime { get; set; }
    public string? PhoneNumber { get; set; }
    public Option? Select { get; set; }
    public Option[]? MultiSelect { get; set; }
    public decimal? Number { get; set; }
    public string? Email { get; set; }
    public Formula1? Formula { get; set; }
    public Uri? Url { get; set; }
    public File1[]? Files { get; set; }
    public Rollup1? Rollup { get; set; }
}

public class Rollup1
{
    public string? Type { get; set; }
    public object[]? Array { get; set; }
    public string? Function { get; set; }
}

public class Date1
{
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}

public class Formula1
{
    public string? Type { get; set; }
    public string? String { get; set; }
}

public class Relation1
{
    public Guid? Id { get; set; }
}

public class People1
{
    public string? Object { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Type { get; set; }
    public Person1? Person { get; set; }
}

public class Person1
{
    public string? Email { get; set; }
}


public class File1
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public InternalFile? File { get; set; }
}

public class InternalFile
{
    public string? Url { get; set; }
    public DateTime ExpiryTime { get; set; }
}