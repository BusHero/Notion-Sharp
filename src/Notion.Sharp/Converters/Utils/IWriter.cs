using System.Text.Json;

namespace Notion.Converters
{
    internal interface IWriter<T>
    {
        string Property { get; }

        void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }
}
