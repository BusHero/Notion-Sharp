using Notion.Model;

namespace MarkdownExporter;

public interface IAplicable
{
    string Apply(RichText richText, string result);

    public static IAplicable operator +(IAplicable first, IAplicable second) => Applicable.ToApplicable((richText, result) => second
        .Apply(richText, first.Apply(richText, result)));
}
