using Notion.Model;

namespace MarkdownExporter;

public static class Formatters
{
    public static string FormatBold(string text) => $"*{text}*";
    public static string FormatItalic(string text) => $"**{text}**";
    public static string FormatStike(string text) => $"~~{text}~~";
    public static string FormatColor(Color color, string text) => $"<span style=\"color: {Enum.GetName(color).ToLower()}\">{text}</span>";
    public static string FormatCode(string text) => $"`{text}`";
    public static string FormatUnderline(string text) => $"<u>{text}</u>";
}
