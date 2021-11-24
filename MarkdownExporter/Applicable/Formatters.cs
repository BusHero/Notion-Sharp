using Notion.Model;

namespace MarkdownExporter;

public static class Formatters
{
    public static string FormatBold(string text) => $"*{text}*";
    public static string FormatItalic(string text) => $"**{text}**";
    public static string FormatStike(string text) => $"~~{text}~~";

    public static string FormatLink(Uri uri, string arg) => $"[{arg}]({uri})";

    public static string FormatColor(Color color, string text) => $"<span style=\"color: {Enum.GetName(color)?.ToLower()}\">{text}</span>";
    public static string FormatCode(string text) => $"`{text}`";
    public static string FormatUnderline(string text) => $"<u>{text}</u>";
    public static string FormatParagraph(string text) => text;
    public static string FormatHeading1(string text) => $"# {text}";
    public static string FormatHeading2(string text) => $"## {text}";
}
