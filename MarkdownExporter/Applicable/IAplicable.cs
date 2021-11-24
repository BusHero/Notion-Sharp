using Notion.Model;

namespace MarkdownExporter;

public interface IAplicable
{
    string Apply(RichText richText, string result);

    public static IAplicable operator +(IAplicable first, IAplicable second) => Applicable.ToApplicable((richText, result) => second
        .Apply(richText, first.Apply(richText, result)));
}

public static class Applicable
{
    public static IAplicable ToApplicable(Func<RichText, string, string> func) => new RelayAplicable(func);

    public static IAplicable ToAplicable(Func<RichText, string> func) => new RelayAplicable((richText, _) => func(richText));

    public static IAplicable ToApplicable(
        Func<RichText, bool> predicate,
        Func<string, string> whenTrue,
        Func<string, string> whenFalse) => new RelayApplicable(
            predicate, whenTrue, whenFalse);

    public static IAplicable ToApplicable(
        Func<RichText, bool> predicate,
        Func<RichText, Func<string, string>> whenTrue,
        Func<RichText, Func<string, string>> whenFalse) => new RelayApplicable2(
            predicate, whenTrue, whenFalse);

    private static T Id<T>(T item) => item;

    public static IAplicable Link(Func<Uri, string, string> formatter) =>
        ToApplicable(
            PropertyAccessor.Link, 
            richText => text => formatter(richText.Href, text),
            _ => Id<string>);

    public static IAplicable Italic(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsItalic, formatter, Id<string>);

    public static IAplicable Strikethrough(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsStriked, formatter, Id<string>);

    public static IAplicable Bold(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsBold, formatter, Id<string>);

    public static IAplicable Underline(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsUnderline, formatter, Id<string>);

    public static IAplicable FormatCode(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsCode, formatter, Id<string>);

    public static IAplicable FormatColor(Func<Color, string, string> formatter) =>
        ToApplicable(
            PropertyAccessor.IsColored,
            richText => text => formatter(richText.Annotations.Color, text),
            _ => Id<string>);

    internal record RelayAplicable(
        Func<RichText, string, string> Relay) : IAplicable
    {
        public string Apply(RichText richText, string result) => Relay(richText, result);
    }

    internal record RelayApplicable(
        Func<RichText, bool> Predicate,
        Func<string, string> WhenTrue,
        Func<string, string> WhenFalse) : IAplicable
    {
        public string Apply(RichText richText, string result) => Predicate(richText) switch
        {
            true => WhenTrue(result),
            false => WhenFalse(result)
        };
    }

    public record RelayApplicable2(
        Func<RichText, bool> Predicate,
        Func<RichText, Func<string, string>> WhenTrue,
        Func<RichText, Func<string, string>> WhenFalse) : IAplicable
    {
        public string Apply(RichText richText, string result) => Predicate(richText) switch
        {
            true => WhenTrue(richText)(result),
            false => WhenFalse(richText)(result)
        };
    }
}
