using Notion.Model;

using Pevac;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Void = Pevac.Void;

namespace Notion.Converters
{
    internal class ParentConverter : JsonConverter<Parent>
    {
        public override Parent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parser.ParseObject<Parent>(propertyName => propertyName switch
            {
                "type" => Parser.String.Updater<string, Parent>(),
                "workspace" => Parser.TrueToken.Updater((Void _, Parent _) => new Parent.Workspace()),
                "page_id" => Parser.Guid.Updater((Guid id, Parent _) => new Parent.Page() { Id = id }),
                "database_id" => Parser.Guid.Updater((Guid id, Parent _) => new Parent.Database() { Id = id }),
                _ => Parser.FailUpdate<Parent>()
            }).Parse(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, Parent value, JsonSerializerOptions options)
        {
            if (Writers is null || !Writers.TryGetValue(value.GetType(), out var parentWriter))
                throw new JsonException($"Cannot serialize {value.GetType().Name}");

            writer.WriteStartObject();
            writer.WritePropertyName(parentWriter.Property);
            parentWriter.Write(writer, value, options);
            writer.WriteEndObject();
        }

        public Dictionary<Type, IWriter<Parent>> Writers { get; init; }
    }
}
