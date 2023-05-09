using System.Text.Json.Nodes;

namespace Notion.Sharp.Tests;

internal static class JsonNodeExtensions
{
    public static void IgnoreProperty(this JsonNode node, string property)
    {
        var words = property.Split(".");
        var last = words.Last();
        var temp = node;
        foreach (var word in words.SkipLast(1))
        {
            temp = temp?[word];
            if (temp is null)
            {
                return;
            }
        }

        if (temp[last] is not null)
        {
            temp[last] = "Ignore";
        }
    }
}