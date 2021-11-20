using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class Exporter
{
    private INotion Client { get; }
    public IConverter<RichText> RichTextConverter { get; }

    public IConverter<Block> BlockConverter { get; } =
        Converters.ToConverter<Block, Block.Heading1>(heading1 =>
        {
            if (heading1.Text[0] is RichText.Text text)
                return Formatters.FormatHeading1(text.Content).ToOption();
            return new Option<string>();
        }) + Converters.ToConverter<Block, Block.Heading2>(heading1 =>
        {
            if (heading1.Text[0] is RichText.Text text)
                return Formatters.FormatHeading2(text.Content).ToOption();
            return new Option<string>();
        }) + Converters.ToConverter<Block, Block.Paragraph>(heading1 =>
        {
            if (heading1.Text[0] is RichText.Text text)
                return Formatters.FormatParagraph(text.Content).ToOption();
            return new Option<string>();
        });


    public Exporter(string notionKey, IConverter<RichText> richTextConverter)
    {
        Client = Notion.Notion.NewClient(notionKey);
        RichTextConverter = richTextConverter ?? throw new ArgumentNullException(nameof(richTextConverter));
    }

    public string Convert(Block block) => BlockConverter.Convert(block).ValueOrDefault(string.Empty);

    public string? Convert(RichText richText) => RichTextConverter.Convert(richText).ValueOrDefault(string.Empty);
}

public static class Expressions
{
    public static T Invoke<T>(Func<T> func) => func.Invoke();
}