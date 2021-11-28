namespace MarkdownExporter;

public static class TaskExtenssions
{
    public static Task<T> ToTask<T>(this T t) => Task.FromResult(t);
}
