using System;

namespace Notion.Model;

public record RichText
{
    public string PlainText { get; init; }
    public Uri Href { get; init; }
    public Annotations Annotations { get; init; }

    public T Copy<T>() where T : RichText, new() => new()
    {
        PlainText = PlainText,
        Href = Href,
        Annotations = Annotations
    };

    public static T Copy<T>(RichText original) where T : RichText, new() => original.Copy<T>();

    #region Sub Types

    public record Text : RichText
    {
        public string Content { get; init; }
        public Link Link { get; init; }

        public static Text Copy(RichText richText) => new Text
        {
            PlainText = richText.PlainText,
            Href = richText.Href,
            Annotations = richText.Annotations
        };
    }

    public record Mention : RichText
    { }

    public record PageMention : Mention
    {
        public Guid Id { get; init; }
    }

    public record LinkPreviewMention: Mention
    {
        public Uri Url { get; init; }
    }

    public record DateMention : Mention
    {
        public DateTimeOffset? Start { get; init; }
        public DateTimeOffset? End { get; init; }
    }

    public record DatabaseMention : Mention
    {
        public Guid Id { get; init; }
    }

    public record UserMention : Mention
    {
        public User User { get; init; }
    }

    public record Equation : RichText
    {
        public string Expression { get; init; }
    }

    #endregion
}
