using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class MarkdownExporter
{
    private INotion Client { get; }

    public MarkdownExporter(string notionKey)
    {
        Client = Notion.Notion.NewClient(notionKey);
    }

    public string Convert(Block.Heading1 heading)
    {
        return $"# {(heading.Text[0] as RichText.Text).Content}";
    }

    public string? Convert(RichText richText)
    {
        return RichTextConverter.Convert(richText);
    }

    private readonly IConverter<RichText> RichTextConverter = 
        (new RichTextTextConverter(
            Applicable.Bold(Formatters.FormatBold)
            + Applicable.Italic(Formatters.FormatItalic)
            + Applicable.Strikethrough(Formatters.FormatStike)
            + Applicable.Underline(Formatters.FormatUnderline)
            + Applicable.FormatCode(Formatters.FormatCode)
            + Applicable.FormatColor(Formatters.FormatColor)) as IConverter<RichText>)
        + new RichTextEquationConverter()
        + new RichTextMentionUserConverter();
}

public interface IConverter<in T>
{
    string? Convert(T input);

    public static IConverter<T> operator +(IConverter<T> first, IConverter<T> second) => new AggregateConverter<T>(first, second);

}

public class RichTextTextConverter : IConverter<RichText>
{
    public IAplicable Applier { get; }

    public RichTextTextConverter(IAplicable aplicable) => Applier = aplicable ?? throw new ArgumentNullException(nameof(aplicable));

    public string? Convert(RichText richText) => richText switch
    {
        RichText.Text text => Applier.Apply(richText, text.Content),
        _ => default
    };

}

public static class Formatters
{
    public static string FormatBold(string text) => $"*{text}*";
    public static string FormatItalic(string text) => $"**{text}**";
    public static string FormatStike(string text) => $"~~{text}~~";
    public static string FormatColor(Color color, string text) => $"<span style=\"color: {Enum.GetName(color).ToLower()}\">{text}</span>";
    public static string FormatCode(string text) => $"`{text}`";
    public static string FormatUnderline(string text) => $"<u>{text}</u>";
}

public static class PropertyAccessor
{
    public static bool IsBold(RichText richText) => richText?.Annotations?.Bold is not null and true;
    public static bool IsItalic(RichText richText) => richText?.Annotations?.Italic is not null and true;
    public static bool IsStriked(RichText richText) => richText?.Annotations?.Strikethrough is not null and true;
    public static bool IsCode(RichText richText) => richText?.Annotations?.Code is not null and true;
    public static bool IsColored(RichText richText) => richText?.Annotations?.Color is not null and not Color.Default;
    public static bool IsUnderline(RichText richText) => richText?.Annotations?.Underline is not null and true;
}

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

    public static IAplicable Content { get; } = new RelayAplicable((richText, _) => richText switch
    {
        RichText.Text text => text.Content,
        _ => default
    });

    public static IAplicable Italic(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsItalic, formatter, Id<string>);

    public static IAplicable Strikethrough(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsStriked, formatter, Id<string>);

    public static IAplicable Bold(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsBold, formatter, Id<string>);

    public static IAplicable Underline(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsUnderline, formatter, Id<string>);

    internal static IAplicable FormatCode(Func<string, string> formatter) =>
        ToApplicable(PropertyAccessor.IsCode, formatter, Id<string>);

    internal static IAplicable FormatColor(Func<Color, string, string> formatter) =>
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

public class RichTextEquationConverter : IConverter<RichText>
{
    public string? Convert(RichText richText) => richText switch
    {
        RichText.Equation equation => $"`{equation.Expression}`",
        _ => default
    };
}

public class RichTextMentionUserConverter: IConverter<RichText>
{
    public string? Convert(RichText richText) => richText switch
    {
        RichText.UserMention userMention => $"**@{userMention.User.Name}**",
        _ => default
    };
}

public class AggregateConverter<T> : IConverter<T>
{
    private IConverter<T> First { get; }
    private IConverter<T> Second { get; }

    public AggregateConverter(IConverter<T> first, IConverter<T> second)
    {
        First = first;
        Second = second;
    }

    public string? Convert(T input)
    {
        return First.Convert(input) switch
        {
            null => Second.Convert(input),
            var x => x
        };
    }
}
