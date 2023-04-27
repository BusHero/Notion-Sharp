using System;
using System.Text.Json;
using Notion.Converters.Utils;
using Notion.Model;
using Pevac;

namespace Notion.Converters;

internal class IconConverter: MyJsonConverter<Icon>
{
    public override Icon? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => (propertyName switch
        {
            "type" => Parser.String!.Updater<string, Icon>(),
            "emoji" => Parser.String.Updater((string? value, Icon _) => new Icon.Emoji { Value = value! }),
            "external" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((Uri? uri, Icon.External file) => file with
                {
                    Url = uri
                }),
                _ => Parser.FailUpdate<Icon.External>($"Unknown key external.'{propertyName1}'")
            }, (Icon _) => new Icon.External()),
            "file" => Parser.ParseObject(propertyName1 => (propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((Uri? uri, Icon.File file) => file with
                {
                    Url = uri
                }),
                "expiry_time" => Parser.DateTimeOffset.Updater((DateTimeOffset expiryTime, Icon.File file) => file with
                {
                    ExpiryTime = expiryTime
                }),
                _ => Parser.FailUpdate<Icon.File>($"Unknown key file.'{propertyName1}'")
            })!, (Icon _) => new Icon.File()),
            _ => Parser.FailUpdate<Icon>($"File object does not have key '{propertyName}'")
        })!).Parse(ref reader, options);
    }
}