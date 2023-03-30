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
    public override Block Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Parser.ParseObject(property => property switch
        {
            "object" => Parser.ParseString("block").Updater<string, Block>(),
            "id" => Parser.Guid.Updater((Guid id, Block block) => block with { Id = id }),
            "type" => Parser.String.Updater<string, Block>(),
            "created_time" => Parser.DateTimeOffset.Updater((DateTimeOffset createdTime, Block block) => block with { CreatedTime = createdTime }),
            "last_edited_time" => Parser.DateTimeOffset.Updater((DateTimeOffset lastEditedTime, Block block) => block with { LastEditedTime = lastEditedTime }),
            "has_children" => Parser.Bool.Updater((bool hasChildren, Block block) => block with { HasChildren = hasChildren }),
            "archived" => Parser.Bool.Updater((bool archived, Block block) => block with { Archived = archived }),
            "parent" => Parser.ParseType<Parent>().Updater((Parent parent, Block block) => block with { Parent = parent}),
            "created_by" => Parser.ParseType<CreatedBy>().Updater((CreatedBy? createdBy, Block block) => block with{ CreatedBy = createdBy}),
            "last_edited_by" => Parser.ParseType<LastEditedBy>().Updater((LastEditedBy? lastEditedBy, Block block) => block with {LastEditedBy = lastEditedBy}),
            "paragraph" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Paragraph paragraph) => paragraph with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.Paragraph paragraph) => paragraph with { Color = color }),
                _ => Parser.FailUpdate<Block.Paragraph>($"Unknown key block.paragraph.{propertyName}")
            }, (Block block) => block.Copy<Block.Paragraph>()),
            "heading_1" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading1 heading1) => heading1 with { Text = text }),
                "is_toggleable" => Parser.Bool.Updater((bool isToggleable, Block.Heading1 heading1) => heading1 with {IsToggable = isToggleable}),
                "color" => Parser.String.Updater((string color, Block.Heading1 heading1) => heading1 with { Color = color }),
                _ => Parser.FailUpdate<Block.Heading1>($"Unknown key block.heading_1.{propertyName}")
            }, (Block block) => block.Copy<Block.Heading1>()),
            "heading_2" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading2 heading2) => heading2 with { Text = text }),
                "is_toggleable" => Parser.Bool.Updater((bool isToggleable, Block.Heading2 heading2) => heading2 with {IsToggable = isToggleable}),
                "color" => Parser.String.Updater((string color, Block.Heading2 heading2) => heading2 with { Color = color }),
                _ => Parser.FailUpdate<Block.Heading2>($"Unknown key block.heading_2.{propertyName}")
            }, (Block block) => block.Copy<Block.Heading2>()),
            "heading_3" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Heading3 heading3) => heading3 with { Text = text }),
                "is_toggleable" => Parser.Bool.Updater((bool isToggleable, Block.Heading3 heading3) => heading3 with {IsToggable = isToggleable}),
                "color" => Parser.String.Updater((string color, Block.Heading3 heading3) => heading3 with { Color = color }),
                _ => Parser.FailUpdate<Block.Heading3>($"Unknown key block.heading_3.{propertyName}")
            }, (Block block) => block.Copy<Block.Heading3>()),
            "bulleted_list_item" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.BulletedListItem bulletedListItem) => bulletedListItem with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.BulletedListItem bulletedListItem) => bulletedListItem with { Color = color }),
                _ => Parser.FailUpdate<Block.BulletedListItem>($"Unknown key block.bulleted_list_item.{propertyName}")
            }, (Block block) => block.Copy<Block.BulletedListItem>()),
            "numbered_list_item" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.NumberedListItem numberedListItem) => numberedListItem with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.NumberedListItem numberedListItem) => numberedListItem with { Color = color }),
                _ => Parser.FailUpdate<Block.NumberedListItem>($"Unknown key block.numbered_list_item.{propertyName}")
            }, (Block block) => block.Copy<Block.NumberedListItem>()),
            "to_do" => Parser.ParseObject(propertyName => propertyName switch
            {
                "text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.ToDo toDo) => toDo with { Text = text }),
                "checked" => Parser.Bool.Updater((bool @checked, Block.ToDo toDo) => toDo with { Checked = @checked }),
                "children" => Parser.ParseType<Block[]>().Updater((Block[] children, Block.ToDo toDo) => toDo with { Children = children }),
                _ => Parser.FailUpdate<Block.ToDo>($"Unknown key block.to_do.{propertyName}")
            }, (Block block) => block.Copy<Block.ToDo>()),
            "toggle" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Toggle toggle) => toggle with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.Toggle toggle) => toggle with { Color = color }),
                _ => Parser.FailUpdate<Block.Toggle>($"Unknown key block.toggle.{propertyName}")
            }, (Block block) => block.Copy<Block.Toggle>()),
            "child_page" => Parser.ParseObject(propertyName => propertyName switch
            {
                "title" => Parser.String.Updater((string title, Block.ChildPage childPage) => childPage with { Title = title }),
                _ => Parser.FailUpdate<Block.ChildPage>($"Unknown key block.child_page.{propertyName}")
            }, (Block block) => block.Copy<Block.ChildPage>()),
            "embed" => Parser.ParseObject(propertyName => propertyName switch
            {
                "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, Block.Embed embed) => embed with { Caption = caption }),
                "url" => Parser.Uri.Updater((Uri url, Block.Embed embed) => embed with { Url = url }),
                _ => Parser.FailUpdate<Block.Embed>($"Unknown key block.embed.{propertyName}")
            }, (Block block) => block.Copy<Block.Embed>()),
            "image" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Image>() with { File = file }),
            "video" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Video>() with { File = file }),
            "audio" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Audio>() with {File = file}),
            "file" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.FileBlock>() with { File = file }),
            "pdf" => Parser.ParseType<File>().Updater((File file, Block block) => block.Copy<Block.Pdf>() with { File = file }),
            "bookmark" => Parser.ParseObject(propertyName => propertyName switch
            {
                "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, Block.Bookmark bookmark) => bookmark with { Caption = caption }),
                "url" => Parser.Uri.Updater((Uri url, Block.Bookmark bookmark) => bookmark with { Url = url }),
                _ => Parser.FailUpdate<Block.Bookmark>($"Unknown key block.bookmark.{propertyName}")
            }, (Block block) => block.Copy<Block.Bookmark>()),
            "child_database" => Parser.ParseObject(propertyName => propertyName switch
            {
                "title" => Parser.String.Updater((string title, Block.ChildDatabase childDatabase) => childDatabase with { Title = title }),
                _ => Parser.FailUpdate<Block.ChildDatabase>($"Unknown key block.title.{propertyName}")
            }, (Block block) => block.Copy<Block.ChildDatabase>()),
            "callout" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Callout callout) => callout with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.Callout callout) => callout with { Color = color }),
                "icon" => Parser.ParseType<Emoji>().Updater((Emoji icon, Block.Callout callout) => callout with { Icon = icon }),
                _ => Parser.FailUpdate<Block.Callout>($"Unknown key block.callout.{propertyName}")
            }, (Block block) => block.Copy<Block.Callout>()),
            "quote" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Quote quote) => quote with { Text = text }),
                "color" => Parser.String.Updater((string color, Block.Quote quote) => quote with { Color = color }),
                _ => Parser.FailUpdate<Block.Quote>($"Unknown key block.quote.{propertyName}")
            }, (Block block) => block.Copy<Block.Quote>()),
            "code" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] text, Block.Code code) => code with { Text = text }),
                "language" => Parser.String.Updater((string language, Block.Code code) => code with { Language = language }),
                "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, Block.Code code) => code with {Caption = caption}),
                _ => Parser.FailUpdate<Block.Code>($"Unknown key block.code.{propertyName}")
            }, (Block block) => block.Copy<Block.Code>()),
            "equation" => Parser.ParseObject(propertyName => propertyName switch
            {
                "expression" => Parser.String.Updater((string expression, Block.Equation equation) => equation with { Expression = expression }),
                _ => Parser.FailUpdate<Block.Equation>($"Unknown key block.equation.{propertyName}")
            }, (Block block) => block.Copy<Block.Equation>()),
            "divider" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Divider>()),
            "table_of_contents" => Parser.ParseObject(propertyName => propertyName switch
            {
                "color" => Parser.String.Updater((string color, Block.TableOfContents tableOfContents) => tableOfContents with { Color = color }),
                _ => Parser.FailUpdate<Block.TableOfContents>($"Unknown key block.equation.{propertyName}")
            }, (Block block) => block.Copy<Block.TableOfContents>()),
            "breadcrumb" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Breadcrumb>()),
            "unsupported" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Unsupported>()),
            "column_list" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.ColumnList>()),
            "column" => Parser.EmptyObject.Updater((Void _, Block block) => block.Copy<Block.Column>()),
            "link_preview" => Parser.ParseObject(propertyName => propertyName switch
            {
                "url" => Parser.Uri.Updater((Uri uri, Block.LinkPreview linkPreview) => linkPreview with { Url = uri }),
                _ => Parser.FailUpdate<Block.LinkPreview>($"Unknown key block.link_preview.{propertyName}")
            }, (Block block) => block.Copy<Block.LinkPreview>()),
            "link_to_page" => Parser.ParseObject(propertyName => propertyName switch
            {
                "type" => Parser.String.Updater<string, Block.LinkToPage>(),
                "page_id" => Parser.Guid.Updater((Guid pageId, Block.LinkToPage linkToPage) => linkToPage.Copy<Block.PagePageLink>() with { PageId = pageId }),
                "database_id" => Parser.Guid.Updater((Guid databaseId, Block.LinkToPage linkToPage) => linkToPage.Copy<Block.DatabasePageLink>() with {DatabaseId = databaseId}),
                _ => Parser.FailUpdate<Block.LinkToPage>($"Unknown key block.link_to_page.{propertyName}")
            }, (Block block) => block.Copy<Block.LinkToPage>()),
            _ => Parser.FailUpdate<Block>($"Unexpected key block.'{property}'")
        }).Parse(ref reader, options);
}
