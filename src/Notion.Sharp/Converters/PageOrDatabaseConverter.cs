using Notion.Model;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion.Converters;

internal class PageOrDatabaseConverter : MyJsonConverter<PageOrDatabase>
{
    public override PageOrDatabase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var tempReader = reader;
        while (tempReader.Read())
        {
            if (tempReader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Unexpected token type. Expected PropertyName, Actual {Enum.GetName(reader.TokenType)}");
            if (tempReader.GetString() != "object")
                tempReader.Skip();
            tempReader.Read();
            if (tempReader.TokenType != JsonTokenType.String)
                throw new JsonException($"Unexpected token type. Expected String, Actual {Enum.GetName(reader.TokenType)}");
            return tempReader.GetString() switch
            {
                "page" => JsonSerializer.Deserialize<Page>(ref reader, options),
                "database" => JsonSerializer.Deserialize<Database>(ref reader, options),
                var x => throw new JsonException($"Unexpected value for 'object' property. Expected : [\"page\", \"database\"], Actual: \"{x}\"")
            };
        }
        throw new JsonException("What a fuck?");
    }

    public override void Write(Utf8JsonWriter writer, PageOrDatabase value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
