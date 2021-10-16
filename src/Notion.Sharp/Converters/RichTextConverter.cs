using Notion.Model;

using Pevac;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Void = Pevac.Void;

namespace Notion.Converters
{
    internal class RichTextConverter : JsonConverter<RichText>
    {
        public override RichText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parser.ParseObject(propertyName => propertyName switch
            {
                "type" => Parser.String.Updater<string, RichText>(),
                "annotations" => Parser.ParseType<Annotations>().Updater((Annotations annotations, RichText richText) => richText with { Annotations = annotations }),
                "plain_text" => Parser.String.Updater((string plainText, RichText richText) => richText with { PlainText = plainText }),
                "href" => Parser.OptionalUri.Updater<Uri, RichText>(),
                "text" => Parser.ParseObject(propertyName => propertyName switch
                {
                    "content" => Parser.String.Updater((string content, RichText.Text text) => text with { Content = content }),
                    "link" => Parser.ParseType<Link>().Updater((Link link, RichText.Text text) => text with { Link = link }),
                    _ => Parser.FailUpdate<RichText.Text>()
                }, (RichText richText) => richText.Copy<RichText.Text>()),
                "equation" => Parser.ParseObject(propertyName => propertyName switch
                {
                    "expression" => Parser.String.Updater((string expression, RichText.Equation equation) => equation with { Expression = expression }),
                    _ => Parser.FailUpdate<RichText.Equation>()
                }, (RichText richText) => richText.Copy<RichText.Equation>()),
                "mention" => Parser.ParseObject(propertyName => propertyName switch
                {
                    "type" => Parser.String.Updater<string, RichText.Mention>(),
                    "user" => Parser.ParseType<User>().Updater((User user, RichText.Mention mention) => mention.Copy<RichText.UserMention>() with { User = user }),
                    "date" => Parser.ParseObject(propertyName => propertyName switch
                    {
                        "start" => Parser.DateTime.Updater((DateTime start, RichText.DateMention dateMention) => dateMention with { Start = start }),
                        "end" => Parser.OptionalDateTime.Updater((DateTime? end, RichText.DateMention dateMention) => dateMention with { End = end }),
                        _ => Parser.FailUpdate<RichText.DateMention>()
                    }, (RichText.Mention mention) => mention.Copy<RichText.DateMention>()),
                    "page" => Parser.ParseObject(propertyName => propertyName switch
                    {
                        "id" => Parser.Guid.Updater((Guid id, RichText.PageMention pageMention) => pageMention with { Id = id }),
                        _ => Parser.FailUpdate<RichText.PageMention>()
                    }, (RichText.Mention mention) => mention.Copy<RichText.PageMention>()),
                    "database" => Parser.ParseObject(propertyName => propertyName switch
                    {
                        "id" => Parser.Guid.Updater((Guid id, RichText.DatabaseMention databaseMention) => databaseMention with { Id = id }),
                        _ => Parser.FailUpdate<RichText.DatabaseMention>()
                    }, (RichText.Mention mention) => mention.Copy<RichText.DatabaseMention>()),
                    _ => Parser.FailUpdate<RichText.Mention>()
                }, (RichText richText) => richText.Copy<RichText.Mention>()),
                _ => Parser.FailUpdate<RichText>()
            }).Parse(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, RichText value, JsonSerializerOptions options)
        {
            var text = value as RichText.Text;
            writer.WriteStartObject();
            writer.WriteStartObject("text");
            writer.WriteString("content", text.Content);
            writer.WriteEndObject();
            //new
            //{
            //    text = new
            //    {
            //        content = "Brave new world!"
            //    }
            //}

            writer.WriteEndObject();
        }
    }
}
