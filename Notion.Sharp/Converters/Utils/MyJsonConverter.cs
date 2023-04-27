using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion.Converters.Utils;

internal abstract class MyJsonConverter<T> : JsonConverter<T>
{
    public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

    public Dictionary<Type, IWriter<T>> Writers { get; init; } = new();

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var type = value?.GetType() ?? throw new ArgumentNullException(nameof(value));
        if (!Writers.TryGetValue(type, out var propertyWriter))
            throw new JsonException($"Cannot serialize {value.GetType().Name}");

        writer.WriteStartObject();
        writer.WritePropertyName(propertyWriter.Property ?? throw new InvalidOperationException());
        propertyWriter.Write(writer, value, options);
        writer.WriteEndObject();
    }
}
