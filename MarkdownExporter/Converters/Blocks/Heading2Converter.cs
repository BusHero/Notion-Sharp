using Notion.Model;

namespace MarkdownExporter;

public class Heading2Converter : Converter<Block.Heading2>
{
    public Converter<RichText[]> RichTextArrayConverter { get;  }

    public Heading2Converter(Func<string, string> formatter) => RichTextArrayConverter = new RichTextArrayConverter(formatter);

    public Heading2Converter() : this(text => $"## {text}") { }

    public override Option<List<string>> Convert(Block.Heading2 heading2, ConverterSettings? settings) => 
        RichTextArrayConverter.Convert(heading2.Text, settings);
}
