using System;
using System.Text.Json;
using Notion.Model;
using Pevac;

namespace Notion.Converters;

internal class CoverConverter : MyJsonConverter<Cover>
{
    public override Cover? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => (propertyName switch
        {
            "type" => Parser.String!.Updater<string, Cover>(),
            "external" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((Uri? uri, Cover.External file) => file with
                {
                    Url = uri
                }),
                _ => Parser.FailUpdate<Cover.External>($"Unknown key external.'{propertyName1}'")
            }, (Cover _) => new Cover.External()),
            "file" => Parser.ParseObject(propertyName1 => (propertyName1 switch
            {
                "url" => Parser.OptionalUri.Updater((Uri? uri, Cover.File file) => file with
                {
                    Url = uri
                }),
                "expiry_time" => Parser.DateTimeOffset.Updater((DateTimeOffset expiryTime, Cover.File file) => file with
                {
                    ExpiryTime = expiryTime
                }),
                _ => Parser.FailUpdate<Cover.File>($"Unknown key file.'{propertyName1}'")
            })!, (Cover _) => new Cover.File()),
            _ => Parser.FailUpdate<Cover>($"File object does not have key '{propertyName}'")
        })!).Parse(ref reader, options);
    }
}