﻿using Notion.Converters;
using Notion.Model;

using Refit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Notion.Converters.Utils;

namespace Notion;

class SnakeCase: JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var result = name
            .Split('_')
            .Select(x => x.Capitalize())
            .Aggregate((w1, w2) => w1 + w2);
        return result;
    }
}

public static class StringExtensions
{
    public static string Capitalize(this string word)
    {
        return word[..1].ToUpper() + word[1..];
    }
}

public static class Notion
{
    /// <summary>
    /// Creates a new notion client by specifying the bearer token and the version of the 
    /// </summary>
    /// <param name="bearerToken">The token used for authorization purposes.</param>
    /// <param name="version">The version of the API. Default is 2021-08-16</param>
    /// <returns></returns>
    public static INotion NewClient(string bearerToken, string version = "2022-06-28")
    {
        return RestService.For<INotion>("https://api.notion.com/v1/", new RefitSettings
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(bearerToken),
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new ColorConverter(),
                    new UserConverter(),
                    new BlockConverter
                    {
                        Writers = new Dictionary<Type, IWriter<Block>>
                        {
                            [typeof(Block.Paragraph)] = Writer.GetWriter<Block>("paragraph", (writer, value, options) =>
                            {
                                var paragraph = value as Block.Paragraph;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, paragraph?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Heading1)] = Writer.GetWriter<Block>("heading_1", (writer, value, options) =>
                            {
                                var heading1 = value as Block.Heading1;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, heading1?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Heading2)] = Writer.GetWriter<Block>("heading_2", (writer, value, options) =>
                            {
                                var heading2 = value as Block.Heading2;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, heading2?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Heading3)] = Writer.GetWriter<Block>("heading_3", (writer, value, options) =>
                            {
                                var heading3 = value as Block.Heading3;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, heading3?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Callout)] = Writer.GetWriter<Block>("callout", (writer, value, options) =>
                            {
                                var callout = value as Block.Callout;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, callout?.Text, options);
                                if (callout?.Icon is not null)
                                {
                                    writer.WritePropertyName("icon");
                                    JsonSerializer.Serialize(writer, callout.Icon, options);
                                }
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Quote)] = Writer.GetWriter<Block>("quote", (writer, value, options) =>
                            {
                                var quote = value as Block.Quote;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, quote?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.BulletedListItem)] = Writer.GetWriter<Block>("bulleted_list_item", (writer, value, options) =>
                            {
                                var bulletedListItem = value as Block.BulletedListItem;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, bulletedListItem?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.NumberedListItem)] = Writer.GetWriter<Block>("numbered_list_item", (writer, value, options) =>
                            {
                                var numberedListItem = value as Block.NumberedListItem;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, numberedListItem?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.ToDo)] = Writer.GetWriter<Block>("to_do", (writer, value, options) =>
                            {
                                var paragraph = value as Block.ToDo;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, paragraph?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Toggle)] = Writer.GetWriter<Block>("toggle", (writer, value, options) =>
                            {
                                var toggle = value as Block.Toggle;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, toggle?.Text, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Code)] = Writer.GetWriter<Block>("code", (writer, value, options) =>
                            {
                                var code = value as Block.Code;
                                writer.WriteStartObject();
                                writer.WritePropertyName("text");
                                JsonSerializer.Serialize(writer, code?.Text, options);
                                writer.WriteString("language", code?.Language);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.ChildPage)] = Writer.GetWriter<Block>("child_page", (writer, value, _) =>
                            {
                                var childPage = value as Block.ChildPage;
                                writer.WriteStartObject();
                                writer.WriteString("title", childPage!.Title);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.ChildDatabase)] = Writer.GetWriter<Block>("child_database", (writer, value, _) =>
                            {
                                var childDatabase = value as Block.ChildDatabase;
                                writer.WriteStartObject();
                                writer.WriteString("title", childDatabase!.Title);
                                writer.WriteEndObject();
                            }),

                            [typeof(Block.Embed)] = Writer.GetWriter<Block>("embed", (writer, value, _) =>
                            {
                                var embed = value as Block.Embed;
                                writer.WriteStartObject();
                                writer.WriteString("url", embed?.Url?.ToString());
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Image)] = Writer.GetWriter<Block>("image", (writer, value, options) =>
                            {
                                var image = value as Block.Image;
                                JsonSerializer.Serialize(writer, image?.File, options);
                            }),
                            [typeof(Block.Video)] = Writer.GetWriter<Block>("video", (writer, value, options) =>
                            {
                                var image = value as Block.Video;
                                JsonSerializer.Serialize(writer, image?.File, options);
                            }),
                            [typeof(Block.FileBlock)] = Writer.GetWriter<Block>("file", (writer, value, options) =>
                            {
                                var file = value as Block.FileBlock;
                                JsonSerializer.Serialize(writer, file?.File, options);
                            }),
                            [typeof(Block.Pdf)] = Writer.GetWriter<Block>("pdf", (writer, value, options) =>
                            {
                                var pdf = value as Block.Pdf;
                                JsonSerializer.Serialize(writer, pdf?.File, options);
                            }),
                            [typeof(Block.Bookmark)] = Writer.GetWriter<Block>("bookmark", (writer, value, options) =>
                            {
                                var bookmark = value as Block.Bookmark;
                                writer.WriteStartObject();
                                writer.WritePropertyName("caption");
                                JsonSerializer.Serialize(writer, bookmark?.Caption, options);
                                writer.WriteString("url", bookmark?.Url?.ToString());
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Equation)] = Writer.GetWriter<Block>("equation", (writer, value, _) =>
                            {
                                var equation = value as Block.Equation;
                                writer.WriteStartObject();
                                writer.WriteString("expression", equation?.Expression);
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Divider)] = Writer.GetWriter<Block>("divider", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.TableOfContents)] = Writer.GetWriter<Block>("table_of_contents", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Block.Breadcrumb)] = Writer.GetWriter<Block>("breadcrumb", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),

                        }
                    },
                    new RichTextConverter
                    {
                        Writers = new Dictionary<Type, IWriter<RichText>>
                        {
                            [typeof(RichText.Text)] = Writer.GetWriter<RichText>("text", (writer, richText, _) =>
                            {
                                var text = richText as RichText.Text;
                                writer.WriteStartObject();
                                writer.WriteString("content", text?.Content);
                                writer.WriteEndObject();
                            }),
                            [typeof(RichText.Equation)] = Writer.GetWriter<RichText>("equation", (writer, richText, _) =>
                            {
                                var text = richText as RichText.Equation;
                                writer.WriteStartObject();
                                writer.WriteString("expression", text?.Expression);
                                writer.WriteEndObject();
                            }),

                            [typeof(RichText.PageMention)] = Writer.GetWriter<RichText>("mention", (writer, richText, _) =>
                            {
                                var pageMention = richText as RichText.PageMention;
                                writer.WriteStartObject();
                                writer.WriteStartObject("page");
                                writer.WriteString("id", pageMention?.Id.ToString());
                                writer.WriteEndObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(RichText.DatabaseMention)] = Writer.GetWriter<RichText>("mention", (writer, richText, _) =>
                            {
                                var databaseMention = richText as RichText.DatabaseMention;
                                writer.WriteStartObject();
                                writer.WriteStartObject("database");
                                writer.WriteString("id", databaseMention?.Id.ToString());
                                writer.WriteEndObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(RichText.DateMention)] = Writer.GetWriter<RichText>("mention", (writer, richText, _) =>
                            {
                                var dateMention = richText as RichText.DateMention;
                                writer.WriteStartObject();
                                writer.WriteStartObject("date");
                                writer.WriteString("start", $"{dateMention?.Start:o}");
                                if (dateMention is { End: not null })
                                    writer.WriteString("end", $"{dateMention.End:o}");
                                writer.WriteEndObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(RichText.UserMention)] = Writer.GetWriter<RichText>("mention", (writer, richText, options) =>
                            {
                                var userMention = richText as RichText.UserMention;
                                writer.WriteStartObject();
                                writer.WritePropertyName("user");
                                JsonSerializer.Serialize(writer, userMention?.User, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(RichText.Text)] = Writer.GetWriter<RichText>("text", (writer, richText, _) =>
                            {
                                var text = richText as RichText.Text;
                                writer.WriteStartObject();
                                writer.WriteString("content", text?.Content);
                                writer.WriteEndObject();
                            }),
                        }
                    },
                    new ParentConverter
                    {
                        Writers = new Dictionary<Type, IWriter<Parent>>
                        {
                            [typeof(Parent.Page)] = Writer.GetWriter<Parent>("page_id", (writer, parent, _) =>
                            {
                                var page = parent as Parent.Page;
                                writer.WriteStringValue(page?.Id.ToString());
                            }),
                            [typeof(Parent.Workspace)] = Writer.GetWriter<Parent>("workspace", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Parent.Database)] = Writer.GetWriter<Parent>("database_id", (writer, parent, _) =>
                            {
                                var page = parent as Parent.Database;
                                writer.WriteStringValue(page?.Id.ToString());
                            })
                        }
                    },
                    new PropertyConverter
                    {
                        Writers = new Dictionary<Type, IWriter<Property>>
                        {
                            [typeof(Property.Title)] = Writer.GetWriter<Property>("title", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.RichTextProperty)] = Writer.GetWriter<Property>("rich_text", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Number)] = Writer.GetWriter<Property>("number", (writer, property, _) =>
                            {
                                var number = property as Property.Number;

                                writer.WriteStartObject();
                                writer.WriteString("format", number?.Format);
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Select)] = Writer.GetWriter<Property>("select", (writer, property, options) =>
                            {
                                var select = property as Property.Select;

                                writer.WriteStartObject();
                                writer.WritePropertyName("options");
                                JsonSerializer.Serialize(writer, select?.Options, options);
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.MultiSelect)] = Writer.GetWriter<Property>("multi_select", (writer, property, options) =>
                            {
                                var select = property as Property.MultiSelect;

                                writer.WriteStartObject();
                                writer.WritePropertyName("options");
                                JsonSerializer.Serialize(writer, select?.Options, options);
                                writer.WriteEndObject();
                            }),

                            [typeof(Property.Date)] = Writer.GetWriter<Property>("date",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.People)] = Writer.GetWriter<Property>("people", (writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Files)] = Writer.GetWriter<Property>("files",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Checkbox)] = Writer.GetWriter<Property>("checkbox",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Url)] = Writer.GetWriter<Property>("url",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Email)] = Writer.GetWriter<Property>("email",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.PhoneNumber)] = Writer.GetWriter<Property>("phone_number",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Formula)] = Writer.GetWriter<Property>("formula", (writer, property, _) =>
                            {
                                var formula = property as Property.Formula;

                                writer.WriteStartObject();
                                writer.WriteString("expression", formula?.Expression);
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Relation)] = Writer.GetWriter<Property>("relation", (writer, property, _) =>
                            {
                                var relation = property as Property.Relation;

                                writer.WriteStartObject();
                                if (relation != null) writer.WriteString("database_id", relation.DatabaseId);
                                //writer.WriteString("synced_property_name", relation.SyncedPropertyName);
                                //writer.WriteString("synced_property_id", relation.SyncedPropertyId);
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.Rollup)] = Writer.GetWriter<Property>("rollup", (writer, property, _) =>
                            {
                                var rollup = property as Property.Rollup;

                                writer.WriteStartObject();
                                writer.WriteString("rollup_property_name", rollup?.RollupPropertyName);
                                writer.WriteString("relation_property_name", rollup?.RelationPropertyName);
                                writer.WriteString("rollup_property_id", rollup?.RollupPropertyId);
                                writer.WriteString("relation_property_id", rollup?.RelationPropertyId);
                                writer.WriteString("function", rollup?.Function);
                                writer.WriteEndObject();
                            }),

                            [typeof(Property.CreatedTime)] = Writer.GetWriter<Property>("created_time",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.CreatedBy)] = Writer.GetWriter<Property>("created_by",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.LastEditedTime)] = Writer.GetWriter<Property>("last_edited_time",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                            [typeof(Property.LastEditedBy)] = Writer.GetWriter<Property>("last_edited_by",(writer, _, _) =>
                            {
                                writer.WriteStartObject();
                                writer.WriteEndObject();
                            }),
                        }
                    },
                    new PropertyValueConverter
                    {
                        Writers = new Dictionary<Type, IWriter<PropertyValue>>
                        {
                            [typeof(PropertyValue.Title)] = Writer.GetWriter<PropertyValue>("title", (writer, propertyValue, options) =>
                            {
                                var title = propertyValue as PropertyValue.Title;
                                JsonSerializer.Serialize(writer, title?.Content, options);
                            }),
                            [typeof(PropertyValue.Text)] = Writer.GetWriter<PropertyValue>("rich_text", (writer, propertyValue, options) =>
                            {
                                var text = propertyValue as PropertyValue.Text;
                                JsonSerializer.Serialize(writer, text?.Content, options);
                            }),
                            [typeof(PropertyValue.Number)] = Writer.GetWriter<PropertyValue>("number", (writer, propertyValue, _) =>
                            {
                                if (propertyValue is PropertyValue.Number { Value: not null, } number)
                                    writer.WriteNumberValue(number.Value.Value);
                                else
                                    writer.WriteNullValue();
                            }),
                            [typeof(PropertyValue.Select)] = Writer.GetWriter<PropertyValue>("select", (writer, propertyValue, options) =>
                            {
                                var select = propertyValue as PropertyValue.Select;
                                JsonSerializer.Serialize(writer, select?.Option, options);
                            }),
                            [typeof(PropertyValue.MultiSelect)] = Writer.GetWriter<PropertyValue>("multi_select", (writer, propertyValue, options) =>
                            {
                                var multiSelect = propertyValue as PropertyValue.MultiSelect;
                                JsonSerializer.Serialize(writer, multiSelect?.Options, options);
                            }),
                            [typeof(PropertyValue.People)] = Writer.GetWriter<PropertyValue>("people", (writer, propertyValue, options) =>
                            {
                                var people = propertyValue as PropertyValue.People;
                                JsonSerializer.Serialize(writer, people?.Value, options);
                            }),
                            [typeof(PropertyValue.Files)] = Writer.GetWriter<PropertyValue>("files", (writer, propertyValue, options) =>
                            {
                                var files = propertyValue as PropertyValue.Files;
                                JsonSerializer.Serialize(writer, files?.Value, options);
                            }),
                            [typeof(PropertyValue.Checkbox)] = Writer.GetWriter<PropertyValue>("checkbox", (writer, propertyValue, _) =>
                            {
                                var checkbox = propertyValue as PropertyValue.Checkbox;
                                writer.WriteBooleanValue(checkbox is { Checked: true, });
                            }),
                            [typeof(PropertyValue.Email)] = Writer.GetWriter<PropertyValue>("email", (writer, propertyValue, _) =>
                            {
                                var email = propertyValue as PropertyValue.Email;
                                writer.WriteStringValue(email?.Value);
                            }),
                            [typeof(PropertyValue.PhoneNumber)] = Writer.GetWriter<PropertyValue>("phone_number", (writer, propertyValue, _) =>
                            {
                                var phoneNumber = propertyValue as PropertyValue.PhoneNumber;
                                writer.WriteStringValue(phoneNumber?.Value);
                            }),
                            [typeof(PropertyValue.Relation)] = Writer.GetWriter<PropertyValue>("relation", (writer, propertyValue, options) =>
                            {
                                var relation = propertyValue as PropertyValue.Relation;
                                JsonSerializer.Serialize(writer, relation?.Pages, options);
                            }),
                            [typeof(PropertyValue.CreatedTime)] = Writer.GetWriter<PropertyValue>("created_time", (writer, propertyValue, options) =>
                            {
                                if (propertyValue is PropertyValue.CreatedTime createdTime)
                                    JsonSerializer.Serialize(writer, createdTime.Value, options);
                            }),
                            [typeof(PropertyValue.CreatedBy)] = Writer.GetWriter<PropertyValue>("created_by", (writer, propertyValue, options) =>
                            {
                                var createdBy = propertyValue as PropertyValue.CreatedBy;
                                JsonSerializer.Serialize(writer, createdBy?.Value, options);
                            }),
                            [typeof(PropertyValue.LastEditedTime)] = Writer.GetWriter<PropertyValue>("last_edited_time", (writer, propertyValue, options) =>
                            {
                                if (propertyValue is PropertyValue.LastEditedTime lastEditedTime)
                                    JsonSerializer.Serialize(writer, lastEditedTime.Value, options);
                            }),
                            [typeof(PropertyValue.LastEditedBy)] = Writer.GetWriter<PropertyValue>("last_edited_by", (writer, propertyValue, options) =>
                            {
                                var lastEditedBy = propertyValue as PropertyValue.LastEditedBy;
                                JsonSerializer.Serialize(writer, lastEditedBy?.Value, options);
                            }),
                            [typeof(PropertyValue.Url)] = Writer.GetWriter<PropertyValue>("url", (writer, propertyValue, _) =>
                            {
                                var url = propertyValue as PropertyValue.Url;
                                writer.WriteStringValue(url?.Link?.ToString());
                            }),
                            [typeof(PropertyValue.Date)] = Writer.GetWriter<PropertyValue>("date", (writer, propertyValue, _) =>
                            {
                                var date = propertyValue as PropertyValue.Date;
                                writer.WriteStartObject();
                                if (date is not null)
                                {
                                    writer.WriteString("start", date.Start?.ToString());
                                    writer.WriteString("end", date.End?.ToString());
                                }

                                writer.WriteEndObject();
                            }),
                        },
                    },
                    new PageOrDatabaseConverter(),
                    new FileConverter()
                    {
                        Writers = new Dictionary<Type, IWriter<File>>
                        {
                            [typeof(File.External)] = Writer.GetWriter<File>("external", (writer, file, _) =>
                            {
                                var externalFile = file as File.External;
                                writer.WriteStartObject();
                                writer.WriteString("url", externalFile?.Uri?.ToString());
                                writer.WriteEndObject();
                            })
                        }
                    },
                    new IconConverter(),
                    new CoverConverter(),
                }
            }),
            ExceptionFactory = GetException,
            HttpMessageHandlerFactory = () => new AuthHeaderHandler(version)
            {
                InnerHandler = new HttpClientHandler()
            }
        });
    }

    private record ErrorDto(string Message, string Code, int Status);

    private static async Task<Exception?> GetException(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
            return await Task.FromResult(default(Exception));

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var error = await JsonSerializer.DeserializeAsync<ErrorDto>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var exception = new NotionException(error?.Message!)
        {
            Code = error?.Code!,
            Status = error!.Status,
        };
        return await Task.FromResult(exception);
    }

    private class AuthHeaderHandler : DelegatingHandler
    {
        public AuthHeaderHandler(string version)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        private string Version { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Notion-Version", Version);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
