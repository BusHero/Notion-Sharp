using Notion.Model;

using Pevac;

using System;
using System.Text.Json;
using Notion.Converters.Utils;
using Void = Pevac.Void;

namespace Notion.Converters;

internal class ParentConverter : MyJsonConverter<Parent>
{
    public override Parent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject<Parent>(propertyName => (propertyName switch
        {
            "type" => Parser.String!.Updater<string, Parent>(),
            "workspace" => Parser.TrueToken.Updater((Void _, Parent _) => new Parent.Workspace()),
            "page_id" => Parser.Guid.Updater((Guid id, Parent _) => new Parent.Page { Id = id }),
            "block_id" => Parser.Guid.Updater((Guid id, Parent _) => new Parent.Block { Id = id }),
            "database_id" => Parser.Guid.Updater((Guid id, Parent _) => new Parent.Database { Id = id }),
            _ => Parser.FailUpdate<Parent>($"Unexpected key parent.{propertyName}")
        })!).Parse(ref reader, options)!;
    }
}
