using Notion.Model;

using Pevac;

using System;
using System.Text.Json;

namespace Notion.Converters;

internal class FileConverter : MyJsonConverter<File>
{
    public override File Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => propertyName switch
        {
            "caption" => Parser.ParseType<RichText[]>().Updater((RichText[] caption, File file) => file with { Caption = caption }),
            "type" => Parser.String.Updater<string, File>(),
            "name" => Parser.String.Updater((string name, File file) => file with { Name = name }),
            "external" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((System.Uri uri, File.External file) => file with { Uri = uri }),
                var key => Parser.FailUpdate<File.External>($"Unknown key external.'{key}'")
            }, (File file) => file.Copy<File.External>()),
            "file" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((System.Uri uri, File.Internal file) => file with { Uri = uri }),
                "expiry_time" => Parser.DateTime.Updater((DateTime expiryTime, File.Internal file) => file with { ExpireTime = expiryTime }),
                var key => Parser.FailUpdate<File.Internal>($"Unknown key file.'{key}'")
            }, (File file) => file.Copy<File.Internal>()),
            var key => Parser.FailUpdate<File>($"File object does not have key '{key}'")
        }).Parse(ref reader, options);
    }
}
