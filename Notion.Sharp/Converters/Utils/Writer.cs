
using System;
using System.Text.Json;

namespace Notion.Converters;

internal static class Writer
{
    internal static IWriter<T> GetWriter<T>(string property, Action<Utf8JsonWriter, T, JsonSerializerOptions?> writer) => (property, writer) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(property)),
        (_, null) => throw new ArgumentNullException(nameof(writer)),
        _ => new DelegateWriter<T>
        {
            Property = property,
            Writer = writer
        }
    };

    private class DelegateWriter<T> : IWriter<T>
    {
        public string Property { get; init; }

        public Action<Utf8JsonWriter, T, JsonSerializerOptions?> Writer { get; init; }

        public void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Writer?.Invoke(writer, value, options);
        }
    }
}
