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
            "external" => Parser.ParseObject(propertyName => propertyName switch
            {
                "url" => Parser.OptionalUri.Updater((System.Uri uri, File.External file) => file with { Uri = uri }),
                var x => Parser.FailUpdate<File.External>($"Unknown key '{x}'")
            }, (File file) => file.Copy<File.External>()),
            "file" => Parser.ParseObject(propertyName => propertyName switch
            {
                "url" => Parser.OptionalUri.Updater((System.Uri uri, File.Internal file) => file with { Uri = uri }),
                "expiry_time" => Parser.DateTime.Updater((DateTime expiryTime, File.Internal file) => file with { ExpireTime = expiryTime }),
                var x => Parser.FailUpdate<File.Internal>($"Unknown key '{x}'")
            }, (File file) => file.Copy<File.Internal>()),
            var x => Parser.FailUpdate<File>($"Unknonwn property '{x}'")
        }).Parse(ref reader, options);
    }
}
