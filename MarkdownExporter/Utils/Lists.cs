namespace MarkdownExporter;

public static class Lists
{
    public static List<T> Of<T>(params T[] items) => items.ToList();

    public static List<T> Of<T>(T item) => new() { item };

    public static List<T> Add<T>(this List<T> first, List<T> second) => first.Concat(second).ToList();

}
