namespace Notion.Sharp.Tests.Utils;

public static class StringExtensions
{
    public static Guid ToGuid(this string guid) => Guid.Parse(guid);
}