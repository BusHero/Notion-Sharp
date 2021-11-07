
using Notion.Model;

using System;

public record PropertyItem
{
    public string @object { get; set; }
    public string type { get; set; }
    public RichText2 title { get; set; }
    public People1 people { get; set; }
    public People1 rich_text { get; set; }
    public Relation1 relation { get; set; }
}

public record RichText2
{
    public string type { get; set; }
    public Text text { get; set; }
    public Annotations annotations { get; set; }
    public string plain_text { get; set; }
    public object href { get; set; }
}

public record Text
{
    public string content { get; set; }
    public object link { get; set; }
}

public record Foo
{
    public string @object { get; set; }
    public PropertyItem[] results { get; set; }
    public object next_cursor { get; set; }
    public bool has_more { get; set; }
    public string type { get; set; }
    public RichText2 title { get; set; }
    public DateTime? created_time { get; set; }
    public Date1 date { get; set; }
    public People1 last_edited_by { get; set; }
    public People1 created_by { get; set; }
    public DateTime? last_edited_time { get; set; }
    public string phone_number { get; set; }
    public Option select { get; set; }
    public Option[] multi_select { get; set; }
    public decimal? number { get; set; }
    public string email { get; set; }
    public Formula1 formula { get; set; }
    public Uri? url { get; set; }
    public File1[] files { get; set; }
    public Rollup1 rollup { get; set; }
}

public class Rollup1
{
    public string type { get; set; }
    public object[] array { get; set; }
    public string function { get; set; }
}

public class Date1
{
    public DateTime? start { get; set; }
    public DateTime? end { get; set; }
}

public class Formula1
{
    public string type { get; set; }
    public string @string { get; set; }
}

public class Relation1
{
    public Guid? id { get; set; }
}

public class People1
{
    public string _object { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public string avatar_url { get; set; }
    public string type { get; set; }
    public Person1 person { get; set; }
}

public class Person1
{
    public string email { get; set; }
}


public class File1
{
    public string name { get; set; }
    public string type { get; set; }
    public InternalFile file { get; set; }
}

public class InternalFile
{
    public string url { get; set; }
    public DateTime expiry_time { get; set; }
}
