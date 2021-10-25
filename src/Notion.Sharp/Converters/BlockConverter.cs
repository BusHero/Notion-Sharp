using Notion.Model;

using Pevac;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Void = Pevac.Void;

namespace Notion.Converters;

internal class BlockConverter : MyJsonConverter<Block>
{
    public override Block Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(property => property switch
        {
            "object" => Parser.ParseString("block").Updater<string, Block>(),
            "id" => Parser.Guid.Updater((Guid id, Block block) => block with { Id = id }),
            "type" => Parser.String.Updater<string, Block>(),
            "created_time" => Parser.DateTime.Updater((DateTime createdTime, Block block) => block with { CreatedTime = createdTime }),
            "last_edited_time" => Parser.DateTime.Updater((DateTime lastEditedTime, Block block) => block with { LastEditedTime = lastEditedTime }),
            "has_children" => Parser.Bool.Updater((bool hasChildren, Block block) => block with { HasChildren = hasChildren }),
            "archived" => Parser.Bool.Updater((bool archived, Block block) => block with { Archived = archived }),
            "paragraph" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Paragraph paragraph) => paragraph with { Text = text }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.Paragraph paragraph) => paragraph with { Children = children }),
                var key => Parser.FailUpdate<Block.Paragraph>($"Unknown key paragraph.{key}")
            }, (Block block) => block.Copy<Block.Paragraph>()),
            "heading_1" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading1 heading1) => heading1 with { Text = text }),
                var key => Parser.FailUpdate<Block.Heading1>($"Unknown key heading_1.{key}")
            }, (Block block) => block.Copy<Block.Heading1>()),
            "heading_2" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading2 heading2) => heading2 with { Text = text }),
                var key => Parser.FailUpdate<Block.Heading2>($"Unknown key heading_2.{key}")
            }, (Block block) => block.Copy<Block.Heading2>()),
            "heading_3" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading3 heading3) => heading3 with { Text = text }),
                var key => Parser.FailUpdate<Block.Heading3>($"Unknown key heading_3.{key}")
            }, (Block block) => block.Copy<Block.Heading3>()),
            "bulleted_list_item" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.BulletedListItem bulletedListItem) => bulletedListItem with { Text = text }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.BulletedListItem bulletedListItem) => bulletedListItem with { Children = children }),
                var key => Parser.FailUpdate<Block.BulletedListItem>($"Unknown key bulleted_list_item.{key}")
            }, (Block block) => block.Copy<Block.BulletedListItem>()),
            "numbered_list_item" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.NumberedListItem numberedListItem) => numberedListItem with { Text = text }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.NumberedListItem numberedListItem) => numberedListItem with { Children = children }),
                var key => Parser.FailUpdate<Block.NumberedListItem>($"Unknown key numbered_list_item.{key}")
            }, (Block block) => block.Copy<Block.NumberedListItem>()),
            "to_do" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.ToDo toDo) => toDo with { Text = text }),
                "checked" => Parser.Bool.Updater((bool @checked, Block.ToDo toDo) => toDo with { Checked = @checked }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.ToDo toDo) => toDo with { Children = children }),
                var key => Parser.FailUpdate<Block.ToDo>($"Unknown key to_do.{key}")
            }, (Block block) => block.Copy<Block.ToDo>()),
            "toggle" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Toggle toggle) => toggle with { Text = text }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.Toggle toggle) => toggle with { Children = children }),
                var key => Parser.FailUpdate<Block.Toggle>($"Unknown key toggle.{key}")
            }, (Block block) => block.Copy<Block.Toggle>()),
            "child_page" => Parser.ParseObject(propertyName => propertyName switch
            {
                "title" => Parser.String.Updater((string title, Block.ChildPage ChildPage) => ChildPage with { Title = title }),
                var key => Parser.FailUpdate<Block.ChildPage>($"Unknown key child_page.{key}")
            }, (Block block) => block.Copy<Block.ChildPage>()),
            "embed" => Parser.ParseObject(propertyName => propertyName switch
            {
                "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, Block.Embed embed) => embed with { Caption = caption }),
                "url" => Parser.Uri.Updater((Uri url, Block.Embed embed) => embed with { Url = url }),
                var key => Parser.FailUpdate<Block.Embed>($"Unknown key embed.{key}")
            }, (Block block) => block.Copy<Block.Embed>()),
            "image" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Image>() with { File = file }),
            "video" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Video>() with { File = file }),
            "file" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.FileBlock>() with { File = file }),
            "pdf" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Pdf>() with { File = file }),
            "bookmark" => Parser.ParseObject(propertyName => propertyName switch
            {
                "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, Block.Bookmark bookmark) => bookmark with { Caption = caption }),
                "url" => Parser.Uri.Updater((Uri url, Block.Bookmark bookmark) => bookmark with { Url = url }),
                var key => Parser.FailUpdate<Block.Bookmark>($"Unknown key bookmark.{key}")
            }, (Block block) => block.Copy<Block.Bookmark>()),
            "child_database" => Parser.ParseObject(propertyName => propertyName switch
            {
                "title" => Parser.String.Updater((string title, Block.ChildDatabase childDatabase) => childDatabase with { Title = title }),
                var key => Parser.FailUpdate<Block.ChildDatabase>($"Unknown key title.{key}")
            }, (Block block) => block.Copy<Block.ChildDatabase>()),
            "callout" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Callout callout) => callout with { Text = text }),
                "icon" => Parser.ParseType<Emoji>().Updater((Emoji icon, Block.Callout callout) => callout with { Icon = icon }),
                var key => Parser.FailUpdate<Block.Callout>($"Unknown key callout.{key}")
            }, (Block block) => block.Copy<Block.Callout>()),
            "quote" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Quote quote) => quote with { Text = text }),
                var key => Parser.FailUpdate<Block.Quote>($"Unknown key quote.{key}")
            }, (Block block) => block.Copy<Block.Quote>()),
            "code" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Code code) => code with { Text = text }),
                "language" => Parser.String.Updater((string language, Block.Code code) => code with { Language = language }),
                var key => Parser.FailUpdate<Block.Code>($"Unknown key code.{key}")
            }, (Block block) => block.Copy<Block.Code>()),
            "equation" => Parser.ParseObject(propertyName => propertyName switch
            {
                "expression" => Parser.String.Updater((string expression, Block.Equation equation) => equation with { Expression = expression }),
                var key => Parser.FailUpdate<Block.Equation>($"Unknown key equation.{key}")
            }, (Block block) => block.Copy<Block.Equation>()),
            "divider" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Divider>()),
            "table_of_contents" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.TableOfContents>()),
            "breadcrumb" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Breadcrumb>()),
            "unsupported" => Parser.Bool.Updater((bool archived, Block block) => block with { Archived = archived }),
            var key => Parser.FailUpdate<Block>($"Unknown key {key}")
        }).Parse(ref reader, options);
    }
}
