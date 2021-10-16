using Notion.Converters;
using Notion.Model;

using Refit;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Notion
{
    public static class Notion
    {
        public static INotion NewClient(string bearerToken, string version)
        {
            return RestService.For<INotion>("https://api.notion.com/v1/", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(bearerToken),
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    Converters =
                    {
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
                                    JsonSerializer.Serialize(writer, paragraph.Text, options);
                                    if (paragraph.Children != null)
                                    {
                                        writer.WritePropertyName("children");
                                        JsonSerializer.Serialize(writer, paragraph.Children, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Heading1)] = Writer.GetWriter<Block>("heading_1", (writer, value, options) =>
                                {
                                    var heading1 = value as Block.Heading1;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, heading1.Text, options);
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Heading2)] = Writer.GetWriter<Block>("heading_2", (writer, value, options) =>
                                {
                                    var heading2 = value as Block.Heading2;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, heading2.Text, options);
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Heading3)] = Writer.GetWriter<Block>("heading_3", (writer, value, options) =>
                                {
                                    var heading3 = value as Block.Heading3;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, heading3.Text, options);
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.BulletedListItem)] = Writer.GetWriter<Block>("bulleted_list_item", (writer, value, options) =>
                                {
                                    var bulletedListItem = value as Block.BulletedListItem;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, bulletedListItem.Text, options);
                                    if (bulletedListItem.Children != null)
                                    {
                                        writer.WritePropertyName("children");
                                        JsonSerializer.Serialize(writer, bulletedListItem.Children, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.NumberedListItem)] = Writer.GetWriter<Block>("numbered_list_item", (writer, value, options) =>
                                {
                                    var numberedListItem = value as Block.NumberedListItem;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, numberedListItem.Text, options);
                                    if (numberedListItem.Children != null)
                                    {
                                        writer.WritePropertyName("children");
                                        JsonSerializer.Serialize(writer, numberedListItem.Children, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.ToDo)] = Writer.GetWriter<Block>("to_do", (writer, value, options) =>
                                {
                                    var paragraph = value as Block.ToDo;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, paragraph.Text, options);
                                    if (paragraph.Children != null)
                                    {
                                        writer.WritePropertyName("children");
                                        JsonSerializer.Serialize(writer, paragraph.Children, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Toggle)] = Writer.GetWriter<Block>("toggle", (writer, value, options) =>
                                {
                                    var toggle = value as Block.Toggle;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, toggle.Text, options);
                                    if (toggle.Children != null)
                                    {
                                        writer.WritePropertyName("children");
                                        JsonSerializer.Serialize(writer, toggle.Children, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Code)] = Writer.GetWriter<Block>("code", (writer, value, options) =>
                                {
                                    var code = value as Block.Code;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, code.Text, options);
                                    writer.WriteString("language", code.Language);
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Quote)] = Writer.GetWriter<Block>("quote", (writer, value, options) =>
                                {
                                    var quote = value as Block.Quote;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, quote.Text, options);
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Bookmark)] = Writer.GetWriter<Block>("bookmark", (writer, value, options) =>
                                {
                                    var bookmark = value as Block.Bookmark;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("caption");
                                    JsonSerializer.Serialize(writer, bookmark.Caption, options);
                                    writer.WriteString("url", bookmark.Url.ToString());
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Callout)] = Writer.GetWriter<Block>("callout", (writer, value, options) =>
                                {
                                    var callout = value as Block.Callout;
                                    writer.WriteStartObject();
                                    writer.WritePropertyName("text");
                                    JsonSerializer.Serialize(writer, callout.Text, options);
                                    if (callout.Icon is not null)
                                    {
                                        writer.WritePropertyName("icon");
                                        JsonSerializer.Serialize(writer, callout.Icon, options);
                                    }
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Divider)] = Writer.GetWriter<Block>("divider", (writer, value, options) =>
                                {
                                    writer.WriteStartObject();
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.TableOfContents)] = Writer.GetWriter<Block>("table_of_contents", (writer, value, options) =>
                                {
                                    writer.WriteStartObject();
                                    writer.WriteEndObject();
                                }),
                                [typeof(Block.Equation)] = Writer.GetWriter<Block>("equation", (writer, value, options) =>
                                {
                                    var equation = value as Block.Equation;
                                    writer.WriteStartObject();
                                    writer.WriteString("expression", equation.Expression);
                                    writer.WriteEndObject();
                                }),
                            }
                        },
                        new RichTextConverter(),
                        new ParentConverter(),
                        new PropertyConverter(),
                        new PropertyValueConverter(),
                        new PageOrDatabaseConverter()
                    }
                }),
                ExceptionFactory = GetException,
                HttpMessageHandlerFactory = () => new AuthHeaderHandler(version)
                {
                    InnerHandler = new HttpClientHandler()
                }
            });
        }

        private record ErrorDTO(string message, string code, int status);

        private static async Task<Exception> GetException(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
                return await Task.FromResult(default(Exception));

            var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var error = await JsonSerializer.DeserializeAsync<ErrorDTO>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var exception = new NotionException(error.message)
            {
                Code = error.code,
                Status = error.status,
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
}
