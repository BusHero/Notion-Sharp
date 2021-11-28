namespace MarkdownExporter;

using Notion.Model;

public static class PaginationListExtensions
{
    public static PaginationList<T> Paginated<T>(this T[] results) => new PaginationList<T> { Results = results };
}
