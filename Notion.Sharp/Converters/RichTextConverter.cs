using Notion.Model;

using Pevac;

using System;
using System.Text.Json;
using Notion.Converters.Utils;

namespace Notion.Converters;

internal class RichTextConverter : MyJsonConverter<RichText>
{
    public override RichText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => propertyName switch
        {
            "type" => Parser.String!.Updater<string, RichText>(),
            "annotations" => Parser.ParseType<Annotations>().Updater((Annotations? annotations, RichText richText) => richText with { Annotations = annotations }),
            "plain_text" => Parser.String.Updater((string? plainText, RichText richText) => richText with { PlainText = plainText }),
            "href" => Parser.OptionalUri!.Updater<Uri, RichText>(),
            "text" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "content" => Parser.String.Updater((string? content, RichText.Text text) => text with { Content = content }),
                "link" => Parser.ParseType<Link>().Updater((Link? link, RichText.Text text) => text with { Link = link }),
                _ => Parser.FailUpdate<RichText.Text>($"Unknown key rich_text.text.{propertyName1}")
            }, (RichText richText) => richText.Copy<RichText.Text>()),
            "equation" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "expression" => Parser.String.Updater((string? expression, RichText.Equation equation) => equation with { Expression = expression }),
                _ => Parser.FailUpdate<RichText.Equation>($"Unknown key rich_text.equation.{propertyName1}")
            }, (RichText richText) => richText.Copy<RichText.Equation>()),
            "mention" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "type" => Parser.String!.Updater<string, RichText.Mention>(),
                "user" => Parser.ParseType<User>().Updater((User? user, RichText.Mention mention) => mention.Copy<RichText.UserMention>() with { User = user }),
                "date" => Parser.ParseObject(propertyName2 => propertyName2 switch
                {
                    "start" => Parser.DateTime.Updater((DateTime start, RichText.DateMention dateMention) => dateMention with { Start = start }),
                    "end" => Parser.OptionalDateTime.Updater((DateTime? end, RichText.DateMention dateMention) => dateMention with { End = end }),
                    "time_zone" => Parser.OptionalString.Updater((string? timeZone, RichText.DateMention dateMention) => dateMention with{TimeZone = timeZone}),
                    _ => Parser.FailUpdate<RichText.DateMention>($"Unknown key rich_text.mention.date.{propertyName2}")
                }, (RichText.Mention mention) => mention.Copy<RichText.DateMention>()),
                "page" => Parser.ParseObject(propertyName2 => propertyName2 switch
                {
                    "id" => Parser.Guid.Updater((Guid id, RichText.PageMention pageMention) => pageMention with { Id = id }),
                    _ => Parser.FailUpdate<RichText.PageMention>($"Unknown key rich_text.mention.page.{propertyName2}")
                }, (RichText.Mention mention) => mention.Copy<RichText.PageMention>()),
                "database" => Parser.ParseObject(propertyName3 => propertyName3 switch
                {
                    "id" => Parser.Guid.Updater((Guid id, RichText.DatabaseMention databaseMention) => databaseMention with { Id = id }),
                    _ => Parser.FailUpdate<RichText.DatabaseMention>($"Unknown key rich_text.mention.database.{propertyName3}")
                }, (RichText.Mention mention) => mention.Copy<RichText.DatabaseMention>()),
                "link_preview" => Parser.ParseObject(propertyName3 => propertyName3 switch
                {
                    "url" => Parser.Uri.Updater((Uri? url, RichText.LinkPreviewMention linkPreview) => linkPreview with { Url = url }),
                    _ => Parser.FailUpdate<RichText.LinkPreviewMention>($"Unknown key rich_text.mention.link_preview.{propertyName3}")
                }, (RichText.Mention mention) => mention.Copy<RichText.LinkPreviewMention>()),
                _ => Parser.FailUpdate<RichText.Mention>($"Unknown key rich_text.mention.{propertyName1}")
            }, (RichText richText) => richText.Copy<RichText.Mention>()),
            _ => Parser.FailUpdate<RichText>($"Unknown key rich_text.{propertyName}")
        }).Parse(ref reader, options);
    }
}
