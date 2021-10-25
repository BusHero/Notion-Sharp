using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion.Converters;

internal abstract class MyJsonConverter<T> : JsonConverter<T>
{
    public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

    public Dictionary<Type, IWriter<T>> Writers { get; init; }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (Writers is null || !Writers.TryGetValue(value.GetType(), out var propertyWriter))
            throw new JsonException($"Cannot serialize {value.GetType().Name}");

        writer.WriteStartObject();
        writer.WritePropertyName(propertyWriter.Property);
        propertyWriter.Write(writer, value, options);
        writer.WriteEndObject();
    }
}
