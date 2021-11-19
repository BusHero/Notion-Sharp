using Notion.Model;

namespace MarkdownExporter;

public static class PropertyAccessor
{
    public static bool IsBold(RichText richText) => richText?.Annotations?.Bold is not null and true;
    public static bool IsItalic(RichText richText) => richText?.Annotations?.Italic is not null and true;
    public static bool IsStriked(RichText richText) => richText?.Annotations?.Strikethrough is not null and true;
    public static bool IsCode(RichText richText) => richText?.Annotations?.Code is not null and true;
    public static bool IsColored(RichText richText) => richText?.Annotations?.Color is not null and not Color.Default;
    public static bool IsUnderline(RichText richText) => richText?.Annotations?.Underline is not null and true;
}