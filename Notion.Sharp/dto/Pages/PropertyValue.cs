using System;
using System.Text.Json.Serialization;
using Notion.experiment;

// ReSharper disable once CheckNamespace
namespace Notion.Model;

public record PropertyValue
{
    public string? Id { get; set; }

    public T Copy<T>() where T : PropertyValue, new() => new()
    {
        Id = Id,
    };

    public static T Copy<T>(PropertyValue property) where T : PropertyValue, new() => property.Copy<T>();

    public record Status : PropertyValue
    {
        public Property.Status.Option? Value { get; set; }
    }
    
    public record Select : PropertyValue
    {
        public Option? Option { get; set; }
    }

    public record MultiSelect : PropertyValue
    {
        public Option[]? Options { get; set; }
    }

    public record Title : PropertyValue
    {
        public RichText[]? Content { get; init; }
    }

    public record Number : PropertyValue
    {
        public double? Value { get; init; }
    }

    public record Text : PropertyValue
    {
        public RichText[]? Content { get; set; }
    }

    public record Checkbox : PropertyValue
    {
        public bool Checked { get; set; }
    }

    public record Url : PropertyValue
    {
        public Uri? Link { get; init; }
    }

    public record Date : PropertyValue
    {
        [JsonPropertyName("start")] public DateTime? Start { get; init; }
        [JsonPropertyName("end")] public DateTime? End { get; init; }
        [JsonPropertyName("time_zone")] public string? TimeZone { get; set; }
    }

    public record PageReference
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }
    }

    public record StringFormula : Formula
    {
        public string? Value { get; init; }
    }
    public record NumberFormula : Formula
    {
        public decimal? Value { get; init; }
    }

    public record BooleanFormula : Formula
    {
        public bool? Value { get; init; }
    }


    public record DateFormula : Formula
    {
        public Date1? Value { get; init; }
    }

    public record Formula : PropertyValue;

    public record Relation : PropertyValue
    {
        public PageReference[]? Pages { get; init; }
    }

    public record NumberRollup : Rollup
    {
        public decimal? Value { get; init; }
    }

    public record DateRollup : Rollup
    {
        public DateTime? Value { get; init; }
    }

    public record ArrayRollup : Rollup
    {
        public PropertyValue[]? Value { get; init; }
    }

    public record Rollup : PropertyValue
    {
        public string? Function { get; init; }
    }

    public record Email : PropertyValue
    {
        public string? Value { get; init; }
    }

    public record People : PropertyValue
    {
        public User[]? Value { get; init; }
    }

    public record Files : PropertyValue
    {
        public File[]? Value { get; init; }
    }

    public record PhoneNumber : PropertyValue
    {
        public string? Value { get; init; }
    }

    public record CreatedTime : PropertyValue
    {
        public DateTimeOffset Value { get; init; }
    }

    public record LastEditedTime : PropertyValue
    {
        public DateTimeOffset Value { get; init; }
    }

    public record CreatedBy : PropertyValue
    {
        public User? Value { get; init; }
    }

    public record LastEditedBy : PropertyValue
    {
        public User? Value { get; init; }
    }
}
